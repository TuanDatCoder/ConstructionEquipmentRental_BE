using AutoMapper;
using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using Data.DTOs.PayOS;
using Data.DTOs.Transaction;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Http;
using Net.payOS.Types;
using Repositories.OrderRepos;
using Services.Helper.CustomExceptions;
using Services.OrderItemServices;
using Services.PayOSServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;
        private readonly IPayOSService _payOSService;

        public OrderService(IOrderRepository orderRepository, IOrderItemService orderItemService, IMapper mapper, IPayOSService payOSService)
        {
            _orderRepository = orderRepository;
            _orderItemService = orderItemService;
            _mapper = mapper;
            _payOSService= payOSService;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(OrderRequestDTO orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);
            order.Status = "CONFIRMED";
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            return _mapper.Map<OrderResponseDTO>(createdOrder);
        }

        public async Task<OrderResponseDTO> UpdateOrderAsync(int orderId, OrderRequestDTO orderRequest)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            _mapper.Map(orderRequest, existingOrder);
            existingOrder.CreatedAt = existingOrder.CreatedAt;
            existingOrder.UpdatedAt = DateTime.Now;
            var updatedOrder = await _orderRepository.UpdateOrderAsync(existingOrder);

            return _mapper.Map<OrderResponseDTO>(updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }
            return await _orderRepository.DeleteOrderAsync(orderId);
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            return _mapper.Map<OrderResponseDTO>(order);
        }


      
        public async Task<string> GetPaymentUrl(HttpContext context, int orderId, string redirectUrl)
        {
            var currOrder = await _orderRepository.GetOrderByIdAsync(orderId);

            if (currOrder == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Order does not exist");
            }

            if (currOrder.Status.Equals(OrderStatusEnum.COMPLETED.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The order has already been completed");
            }

            //nếu có link thì trả về luôn
            if (!string.IsNullOrEmpty(currOrder.PayOsUrl)) {
                var link = currOrder.PayOsUrl;
                return link;
            }

            var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);

            PayOSRequestDTO payOSRequestDTO = new PayOSRequestDTO
            {
                OrderId = currOrder.Id,
                Amount = currOrder.TotalPrice,
                RedirectUrl = redirectUrl,
                CancelUrl = redirectUrl,
                OrderItems = orderItems.ToList()
            };

            var result = await _payOSService.createPaymentUrl(payOSRequestDTO);

            currOrder.PayOsUrl = result.checkoutUrl;
            currOrder.PaymentMethod = PaymentMethodEnum.PAYOS.ToString();
            currOrder.Status = OrderStatusEnum.PENDING.ToString();
            currOrder.UpdatedAt = DateTime.Now;

            await _orderRepository.UpdateOrderAsync(currOrder);

            return result.checkoutUrl;

        }
        //public async Task<OrderResponseDTO> CreateOrderWithItemsAsync(OrderWithItemsRequestDTO orderWithItemsRequest)
        //{
        //    // Kiểm tra đầu vào hợp lệ
        //    if (orderWithItemsRequest == null || orderWithItemsRequest.OrderItems == null || !orderWithItemsRequest.OrderItems.Any())
        //    {
        //        throw new ArgumentNullException(nameof(orderWithItemsRequest), "Order request or order items cannot be null or empty.");
        //    }

        //    // Chuyển đổi DTO sang Entity Order
        //    var order = _mapper.Map<Order>(orderWithItemsRequest);
        //    order.Status = OrderStatusEnum.PENDING.ToString();  // Đặt trạng thái mặc định là PENDING
        //    order.CreatedAt = DateTime.Now;
        //    order.UpdatedAt = DateTime.Now;

        //    // Lưu đơn hàng vào cơ sở dữ liệu
        //    var createdOrder = await _orderRepository.CreateOrderAsync(order);

        //    // Thêm các sản phẩm vào đơn hàng
        //    var orderItemList = new List<OrderItem>();
        //    foreach (var orderItemDto in orderWithItemsRequest.OrderItems)
        //    {
        //        // Chuyển đổi OrderItemRequestDTO thành OrderItem Entity
        //        var orderItem = _mapper.Map<OrderItem>(orderItemDto);
        //        orderItem.OrderId = createdOrder.Id; // Liên kết OrderItem với Order vừa tạo
        //        var createdOrderItem = await _orderItemService.CreateOrderItemAsync(orderItem);  // Lưu OrderItem vào cơ sở dữ liệu
        //        orderItemList.Add(createdOrderItem);
        //    }

        //    // Lấy các sản phẩm đã lưu
        //    var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync(createdOrder.Id);

        //    // Chuyển đổi kết quả sang OrderResponseDTO để trả về
        //    var response = _mapper.Map<OrderResponseDTO>(createdOrder);
        //    response.OrderItems = _mapper.Map<List<OrderItemResponseDTO>>(orderItems);  // Chuyển đổi OrderItems sang DTO

        //    return response;
        //}





    }
}
