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
            if (orders == null || !orders.Any())
            {
                return new List<AnnualRevenueDTO>(); // Trả về rỗng nếu không có đơn hàng
            }

            var revenueByYear = orders
                .SelectMany(o => o.OrderItems.Select(oi => new
                {
                    Year = o.CreatedAt.Year,
                    Month = o.CreatedAt.Month,
                    Revenue = oi.Price * oi.Quantity * 0.9m // Lessor nhận 90%, admin lấy 10%
                }))
                .GroupBy(x => x.Year)
                .Select(yearGroup => new AnnualRevenueDTO
                {
                    Year = yearGroup.Key,
                    TotalRevenue = yearGroup.Sum(x => x.Revenue), // Tổng doanh thu của Lessor (90%)
                    MonthlyRevenues = yearGroup
                        .GroupBy(x => x.Month)
                        .Select(monthGroup => new MonthlyRevenueDTO
                        {
                            Year = yearGroup.Key,
                            Month = monthGroup.Key,
                            Revenue = monthGroup.Sum(x => x.Revenue), // Doanh thu tháng của Lessor (90%)
                            PercentageChange = 0 // Sẽ tính sau
                        })
                        .OrderBy(m => m.Month)
                        .ToList()
                })
                .OrderBy(y => y.Year)
                .ToList();

            // Tạo từ điển để truy xuất nhanh doanh thu theo năm và tháng (đã là 90%)
            var revenueLookup = revenueByYear
                .SelectMany(y => y.MonthlyRevenues)
                .ToDictionary(m => (m.Year, m.Month), m => m.Revenue);

            // Tính phần trăm thay đổi so với tháng trước
            foreach (var yearData in revenueByYear)
            {
                for (int i = 0; i < yearData.MonthlyRevenues.Count; i++)
                {
                    var currentMonth = yearData.MonthlyRevenues[i];

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

            var currentDate = DateTime.UtcNow;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;

            var previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            var previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;

            var orders = await _orderRepository.GetOrdersByLessorIdAsync(account.Id);

            // Tính cho tháng hiện tại
            var currentMonthOrders = orders
                .Where(o => o.CreatedAt.Year == currentYear && o.CreatedAt.Month == currentMonth)
                .ToList();
            var totalOrdersCurrent = currentMonthOrders.Count;

            var totalRevenueCurrent = currentMonthOrders
                .SelectMany(o => o.OrderItems)
                .Where(oi => oi.Product.Store.AccountId == account.Id)
                .Sum(oi => oi.Price * oi.Quantity * 0.9m); // Lessor nhận 90%

            // Tính cho tháng trước
            var previousMonthOrders = orders
                .Where(o => o.CreatedAt.Year == previousYear && o.CreatedAt.Month == previousMonth)
                .ToList();
            var totalOrdersPrevious = previousMonthOrders.Count;

            var totalRevenuePrevious = previousMonthOrders
                .SelectMany(o => o.OrderItems)
                .Where(oi => oi.Product.Store.AccountId == account.Id)
                .Sum(oi => oi.Price * oi.Quantity * 0.9m); // Lessor nhận 90%

            // Lấy tổng stock từ bảng Product của Lessor
            var products = await _productRepository.GetProductsByLessorIdAsync(account.Id);
            var totalEquipment = products?.Sum(p => p.Stock) ?? 0;

            // Tính tổng thiết bị đang cho thuê (dựa trên đơn hàng còn hiệu lực)
            var totalRentedEquipmentCurrent = orders
                .Where(o => o.DateOfReceipt.ToDateTime(TimeOnly.MinValue) <= currentDate &&
                            o.DateOfReturn.ToDateTime(TimeOnly.MaxValue) >= currentDate &&
                            o.Status != "CANCELLED") // Giả định đơn hàng chưa hủy
                .SelectMany(o => o.OrderItems)
                .Where(oi => oi.Product.Store.AccountId == account.Id)
                .Sum(oi => oi.Quantity);

            // Tính tổng thiết bị đang cho thuê tháng trước
            var lastDayOfPreviousMonth = new DateTime(previousYear, previousMonth, DateTime.DaysInMonth(previousYear, previousMonth));
            var totalRentedEquipmentPrevious = orders
                .Where(o => o.DateOfReceipt.ToDateTime(TimeOnly.MinValue) <= lastDayOfPreviousMonth &&
                            o.DateOfReturn.ToDateTime(TimeOnly.MaxValue) >= lastDayOfPreviousMonth &&
                            o.Status != "CANCELLED") // Giả định đơn hàng chưa hủy
                .SelectMany(o => o.OrderItems)
                .Where(oi => oi.Product.Store.AccountId == account.Id)
                .Sum(oi => oi.Quantity);

            // Tính phần trăm thay đổi
            double? ordersPercentageChange = totalOrdersPrevious > 0
                ? (double)((totalOrdersCurrent - totalOrdersPrevious) * 100m / totalOrdersPrevious)
                : null;

            double? revenuePercentageChange = totalRevenuePrevious > 0
                ? (double)((totalRevenueCurrent - totalRevenuePrevious) * 100m / totalRevenuePrevious)
                : null;

            double? equipmentPercentageChange = null; // Để null vì không có dữ liệu tháng trước cho stock

            double? rentedEquipmentPercentageChange = totalRentedEquipmentPrevious > 0
                ? (double)((totalRentedEquipmentCurrent - totalRentedEquipmentPrevious) * 100m / totalRentedEquipmentPrevious)
                : null;

            return new LessorSummaryDTO
            {
                TotalOrders = totalOrdersCurrent,
                TotalRevenue = totalRevenueCurrent,
                TotalEquipment = totalEquipment,
                TotalRentedEquipment = totalRentedEquipmentCurrent,
                TotalOrdersPercentageChange = ordersPercentageChange,
                TotalRevenuePercentageChange = revenuePercentageChange,
                TotalEquipmentPercentageChange = equipmentPercentageChange,
                TotalRentedEquipmentPercentageChange = rentedEquipmentPercentageChange
            };
        }



    }
}

