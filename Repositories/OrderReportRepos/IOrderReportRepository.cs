using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OrderReportRepos
{
    public interface IOrderReportRepository
    {
        Task<IEnumerable<OrderReport>> GetAllOrderReportAsync();
        Task<OrderReport> CreateOrderReportAsync(OrderReport orderReport);

        Task<OrderReport> UpdateOrderReportAsync(OrderReport orderReport);

        Task<bool> DeleteOrderReportAsync(int orderReportId);

        Task<OrderReport> GetOrderReportByIdAsync(int orderReportId);
    }
}
