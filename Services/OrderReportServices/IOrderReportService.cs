using Data.DTOs.Order;
using Data.DTOs.OrderReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderReportServices
{
    public interface IOrderReportService
    {
        Task<IEnumerable<OrderReportResponseDTO>> GetAllOrderReportAsync();
        Task<OrderReportResponseDTO> CreateOrderReportAsync(OrderReportRequestDTO orderReportRequest);
        Task<OrderReportResponseDTO> UpdateOrderReportAsync(int orderReportId, OrderReportRequestDTO orderReportRequest);
        Task<bool> DeleteOrderReportAsync(int orderReportId);
        Task<OrderReportResponseDTO> GetOrderReportByIdAsync(int orderReportId);
    }
}
