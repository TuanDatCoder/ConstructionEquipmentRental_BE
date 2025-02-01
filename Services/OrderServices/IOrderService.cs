using Data.DTOs.Order;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDTO>> GetAllOrdersAsync();
        Task<OrderResponseDTO> CreateOrderAsync(OrderRequestDTO orderRequest);
        Task<OrderResponseDTO> UpdateOrderAsync(int orderId, OrderRequestDTO orderRequest);
        Task<bool> DeleteOrderAsync(int orderId);
        Task<OrderResponseDTO> GetOrderByIdAsync(int orderId);
        Task<string> GetPaymentUrl(HttpContext context, int orderId, string redirectUrl);
    }
}
