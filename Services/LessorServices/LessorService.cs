using AutoMapper;
using Data.DTOs.Lessor;
using Microsoft.Extensions.Configuration;
using Repositories.OrderRepos;
using Repositories.ProductRepos;
using Services.OrderItemServices;
using Services.ProductServices;

namespace Services.LessorServices
{
    public class LessorService : ILessorService
    {

        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public LessorService(IConfiguration config, IOrderRepository orderRepository, IOrderItemService orderItemService, IMapper mapper, IProductService productService, IProductRepository productRepository)
        {
            _config = config;
            _orderRepository = orderRepository;
            _orderItemService = orderItemService;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<List<AnnualRevenueDTO>> GetRevenueByLessorAsync(int lessorId)
        {
            var orders = await _orderRepository.GetOrdersByLessorIdAsync(lessorId);

            var revenueByYear = orders
                .SelectMany(o => o.OrderItems.Select(oi => new
                {
                    Year = o.CreatedAt.Year,
                    Month = o.CreatedAt.Month,
                    Revenue = oi.Price * oi.Quantity
                }))
                .GroupBy(x => x.Year)
                .Select(yearGroup => new AnnualRevenueDTO
                {
                    Year = yearGroup.Key,
                    TotalRevenue = yearGroup.Sum(x => x.Revenue),
                    MonthlyRevenues = yearGroup
                        .GroupBy(x => x.Month)
                        .Select(monthGroup => new MonthlyRevenueDTO
                        {
                            Year = yearGroup.Key,
                            Month = monthGroup.Key,
                            Revenue = monthGroup.Sum(x => x.Revenue),
                            PercentageChange = 0 // Sẽ tính sau
                        })
                        .OrderBy(m => m.Month) // Sắp xếp theo tháng
                        .ToList()
                })
                .OrderBy(y => y.Year) // Sắp xếp theo năm
                .ToList();

            // Tạo từ điển để truy xuất nhanh doanh thu theo năm và tháng
            var revenueLookup = revenueByYear
                .SelectMany(y => y.MonthlyRevenues)
                .ToDictionary(m => (m.Year, m.Month), m => m.Revenue);

            // Tính phần trăm thay đổi so với tháng trước
            foreach (var yearData in revenueByYear)
            {
                for (int i = 0; i < yearData.MonthlyRevenues.Count; i++)
                {
                    var currentMonth = yearData.MonthlyRevenues[i];

                    // Xác định tháng trước
                    int prevYear = currentMonth.Month == 1 ? currentMonth.Year - 1 : currentMonth.Year;
                    int prevMonth = currentMonth.Month == 1 ? 12 : currentMonth.Month - 1;

                    if (revenueLookup.TryGetValue((prevYear, prevMonth), out decimal prevRevenue) && prevRevenue > 0)
                    {
                        currentMonth.PercentageChange = ((currentMonth.Revenue - prevRevenue) / prevRevenue) * 100;
                    }
                    else
                    {
                        currentMonth.PercentageChange = null; // Nếu không có dữ liệu tháng trước
                    }
                }
            }

            return revenueByYear;
        }

    }
}

