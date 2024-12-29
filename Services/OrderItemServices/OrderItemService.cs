using AutoMapper;
using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using Data.Entities;
using Repositories.OrderItemRepos;
using Repositories.OrderRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderItemServices
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<OrderItemResponseDTO> CreateOrderItemAsync(OrderItemRequestDTO orderItemRequest)
        {
            // Chuyển từ OrderRequestDTO sang Order
            var orderItem = _mapper.Map<OrderItem>(orderItemRequest);

            // Lưu vào database
            var createdOrderItem = await _orderItemRepository.CreateOrderItemAsync(orderItem);

            // Chuyển từ Order sang OrderResponseDTO để trả về
            return _mapper.Map<OrderItemResponseDTO>(createdOrderItem);
        }

        public async Task<bool> DeleteOrderItemAsync(int orderItemId)
        {
            // Tìm Order trong database
            var existingOrderItem = await _orderItemRepository.GetOrderItemByIdAsync(orderItemId);
            if (existingOrderItem == null)
            {
                throw new Exception($"Order Item with ID {orderItemId} not found.");
            }

            // Xóa Order
            return await _orderItemRepository.DeleteOrderItemAsync(orderItemId);
        }

        public async Task<IEnumerable<OrderItemResponseDTO>> GetAllOrderItemsAsync()
        {
            var orderItems = await _orderItemRepository.GetAllOrderItemAsync();

            // Sử dụng AutoMapper để ánh xạ
            return _mapper.Map<IEnumerable<OrderItemResponseDTO>>(orderItems);
        }

        public async Task<OrderItemResponseDTO> GetOrderItemByIdAsync(int orderItemId)
        {
            var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(orderItemId);
            if (orderItem == null)
            {
                throw new Exception($"Order Item with ID {orderItemId} not found.");
            }

            return _mapper.Map<OrderItemResponseDTO>(orderItem);
        }

        public async Task<OrderItemResponseDTO> UpdateOrderItemAsync(int orderItemId, OrderItemRequestDTO orderItemRequest)
        {
            // Tìm Order trong database
            var existingOrderItem = await _orderItemRepository.GetOrderItemByIdAsync(orderItemId);
            if (existingOrderItem == null)
            {
                throw new Exception($"Order Item with ID {orderItemId} not found.");
            }

            // Map thông tin từ DTO vào entity
            _mapper.Map(orderItemRequest, existingOrderItem);

            var updatedOrderItem = await _orderItemRepository.UpdateOrderItemAsync(existingOrderItem);

            return _mapper.Map<OrderItemResponseDTO>(updatedOrderItem);
        }
    }
}
