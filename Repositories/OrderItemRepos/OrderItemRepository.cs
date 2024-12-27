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
        public async Task<IEnumerable<OrderItem>> GetAllOrderItemAsync()
        {
            return await _context.Set<OrderItem>()
                .Include(o => o.Order)
                .Include(o => o.Product)
                .ToListAsync();
        }
    }
}
