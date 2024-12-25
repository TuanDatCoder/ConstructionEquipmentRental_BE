using AutoMapper;
using Data.DTOs.Order;
using Data.Entities;
using Repositories.OrderRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            // Sử dụng AutoMapper để ánh xạ
            return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(OrderRequestDTO orderRequest)
        {
            // Chuyển từ OrderRequestDTO sang Order
            var order = _mapper.Map<Order>(orderRequest);
            order.Status = "CONFIRMED";
            order.CreatedAt = DateTime.Now;

            // Lưu vào database
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Chuyển từ Order sang OrderResponseDTO để trả về
            return _mapper.Map<OrderResponseDTO>(createdOrder);
        }

        public async Task<OrderResponseDTO> UpdateOrderAsync(int orderId, OrderRequestDTO orderRequest)
        {
            // Tìm Order trong database
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            // Map thông tin từ DTO vào entity
            _mapper.Map(orderRequest, existingOrder);

            // Cập nhật thông tin
            existingOrder.CreatedAt = DateTime.Now; // Thêm cột `UpdatedAt` nếu cần
            var updatedOrder = await _orderRepository.UpdateOrderAsync(existingOrder);

            // Trả về thông tin sau khi cập nhật
            return _mapper.Map<OrderResponseDTO>(updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            // Tìm Order trong database
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            // Xóa Order
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

    }
}
