using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderItemServices
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemResponseDTO>> GetAllOrderItemsAsync();
        Task<OrderItemResponseDTO> CreateOrderItemAsync(OrderItemRequestDTO orderItemRequest);
        Task<OrderItemResponseDTO> UpdateOrderItemAsync(int orderItemId, OrderItemRequestDTO orderItemRequest);
        Task<bool> DeleteOrderItemAsync(int orderItemId);
        Task<OrderItemResponseDTO> GetOrderItemByIdAsync(int orderItemId);
        Task<IEnumerable<OrderItemResponseDTO>> GetOrderItemsByOrderIdAsync(int orderId);

    }
}
