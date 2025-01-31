using AutoMapper;
using Data.DTOs.Order;
using Data.DTOs.PayOS;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Http;
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
            existingOrder.CreatedAt = DateTime.Now;
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



            PayOSRequestDTO payOSRequestDTO = new PayOSRequestDTO
            {
                OrderId = currOrder.Id,
                //productName = currOrder.Course.CourseName,
                Amount = currOrder.TotalPrice,
                RedirectUrl = redirectUrl,
                CancelUrl = redirectUrl
            };

            var result = await _payOSService.createPaymentUrl(payOSRequestDTO);

            return result.checkoutUrl;

        }

    }
}
