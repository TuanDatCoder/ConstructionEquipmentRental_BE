using Data.Entities;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.StoreRepos
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public StoreRepository(ConstructionEquipmentRentalDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _context.Set<Store>()
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .Include(o => o.OrderItems)
                .Include(o => o.Feedbacks)
                .Include(o => o.OrderReports)
                .Include(o => o.Transactions)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Set<Order>().Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

    }
}
