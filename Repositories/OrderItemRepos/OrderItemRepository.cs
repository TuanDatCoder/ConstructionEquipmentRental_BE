using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OrderItemRepos
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public OrderItemRepository(ConstructionEquipmentRentalDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem)
        {
            _context.Set<OrderItem>().Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<bool> DeleteOrderItemAsync(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null)
            {
                return false;
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemAsync()
        {
            return await _context.Set<OrderItem>()
                .Include(o => o.Order)
                .Include(o => o.Product)
                .ToListAsync();
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemId)
        {
            return await _context.OrderItems
                .Include(o => o.Order) 
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == orderItemId);
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }
    }
}
