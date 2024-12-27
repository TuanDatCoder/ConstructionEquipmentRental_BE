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
        //Task<OrderResponseDTO> CreateOrderAsync(OrderRequestDTO orderRequest);
        //Task<OrderResponseDTO> UpdateOrderAsync(int orderId, OrderRequestDTO orderRequest);
        //Task<bool> DeleteOrderAsync(int orderId);
        //Task<OrderResponseDTO> GetOrderByIdAsync(int orderId);
    }
}
