using Data.Entities;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories.OrderReportRepos
{
    public class OrderReportRepository : IOrderReportRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public OrderReportRepository(ConstructionEquipmentRentalDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderReport>> GetAllOrderReportAsync()
        {
            return await _context.Set<OrderReport>()
                .Include(o => o.Handler)
                .Include(o => o.Order)
                .Include(o => o.Reporter)
                .ToListAsync();
        }

        public async Task<OrderReport> CreateOrderReportAsync(OrderReport orderReport)
        {
            _context.Set<OrderReport>().Add(orderReport);
            await _context.SaveChangesAsync();
            return orderReport;
        }

        public async Task<OrderReport> UpdateOrderReportAsync(OrderReport orderReport)
        {
            _context.OrderReports.Update(orderReport);
            await _context.SaveChangesAsync();
            return orderReport;
        }

        public async Task<bool> DeleteOrderReportAsync(int orderReportId)
        {
            var orderReport = await _context.OrderReports.FindAsync(orderReportId);
            if (orderReport == null)
            {
                return false;
            }

            _context.OrderReports.Remove(orderReport);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrderReport> GetOrderReportByIdAsync(int orderReportId)
        {
            return await _context.OrderReports
                .Include(o => o.Handler)
                .Include(o => o.Order)
                .Include(o => o.Reporter)
                .FirstOrDefaultAsync(o => o.Id == orderReportId);
        }

    }
}
