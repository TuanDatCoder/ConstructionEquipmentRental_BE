using AutoMapper;
using Data.DTOs.Lessor;
using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Repositories.OrderRepos;
using Repositories.ProductRepos;
using Services.AuthenticationServices;
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
        private readonly IAuthenticationService _authenticationService;
        private readonly IProductRepository _productRepository;

        public LessorService(IConfiguration config, IOrderRepository orderRepository, IOrderItemService orderItemService, IMapper mapper, IProductService productService, IProductRepository productRepository, IAuthenticationService authenticationService)
        {
            _config = config;
            _orderRepository = orderRepository;
            _orderItemService = orderItemService;
            _productService = productService;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _productRepository = productRepository;
        }

        public async Task<List<AnnualRevenueDTO>> GetRevenueByLessorAsync(string token)
        {
            var account = await _authenticationService.GetAccountByToken(token);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }


            var orders = await _orderRepository.GetOrdersByLessorIdAsync(account.Id);

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


        public async Task<List<OrderAndItemsResponseDTO>> GetOrdersByLessorAsync(string token)
        {

            var account = await _authenticationService.GetAccountByToken(token);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }
            var orders = await _orderRepository.GetOrdersByLessorIdAsync(account.Id);

            var orderDtos = _mapper.Map<List<OrderAndItemsResponseDTO>>(orders);

            foreach (var orderDto in orderDtos)
            {
                orderDto.TotalPrice = orders
                    .First(o => o.Id == orderDto.Id)
                    .OrderItems
                    .Where(oi => oi.Product.Store.AccountId == account.Id)
                    .Sum(oi => oi.Price * oi.Quantity);
            }

            return orderDtos;
        }

        public async Task<LessorSummaryDTO> GetLessorSummaryAsync(string token)
        {
            var account = await _authenticationService.GetAccountByToken(token);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }

            // Lấy danh sách đơn hàng của Lessor
            var orders = await _orderRepository.GetOrdersByLessorIdAsync(account.Id);
            var totalOrders = orders.Count;

            // Tính tổng doanh thu
            var totalRevenue = orders
                .SelectMany(o => o.OrderItems)
                .Where(oi => oi.Product.Store.AccountId == account.Id)
                .Sum(oi => oi.Price * oi.Quantity);

            // Lấy tổng stock từ bảng Product của Lessor
            var products = await _productRepository.GetProductsByLessorIdAsync(account.Id); // await để lấy List<Product>
            var totalEquipment = products.Sum(p => p.Stock); // Tính tổng stock từ các sản phẩm

            return new LessorSummaryDTO
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalEquipment = totalEquipment
            };
        }







    }
}

