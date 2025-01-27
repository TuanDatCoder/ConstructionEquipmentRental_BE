using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OrderItemRepos
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemAsync();
        Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem);

        Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);

        Task<bool> DeleteOrderItemAsync(int orderItemId);

        Task<OrderItem> GetOrderItemByIdAsync(int orderItemId);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);

    }
}
