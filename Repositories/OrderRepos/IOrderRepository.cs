using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OrderRepos
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> CreateOrderAsync(Order order);

        Task<Order> UpdateOrderAsync(Order order);

        Task<bool> DeleteOrderAsync(int orderId);

        Task<Order> GetOrderByIdAsync(int orderId);
    }
}

