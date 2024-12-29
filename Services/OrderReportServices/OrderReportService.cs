using AutoMapper;
using Data.DTOs.Order;
using Data.DTOs.OrderReport;
using Data.Entities;
using Repositories.OrderReportRepos;
using Repositories.OrderRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderReportServices
{
    public class OrderReportService : IOrderReportService
    {

        private readonly IOrderReportRepository _orderReportRepository;
        private readonly IMapper _mapper;

        public OrderReportService(IOrderReportRepository orderReportRepository, IMapper mapper)
        {
            _orderReportRepository = orderReportRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderReportResponseDTO>> GetAllOrderReportAsync()
        {
            var orderReport = await _orderReportRepository.GetAllOrderReportAsync();

            return _mapper.Map<IEnumerable<OrderReportResponseDTO>>(orderReport);
        }

        public async Task<OrderReportResponseDTO> CreateOrderReportAsync(OrderReportRequestDTO orderReportRequest)
        {
            var orderReport = _mapper.Map<OrderReport>(orderReportRequest);
            orderReport.Status = "PENDING";
            orderReport.CreatedAt = DateTime.Now;

            var createdOrder = await _orderReportRepository.CreateOrderReportAsync(orderReport);

            return _mapper.Map<OrderReportResponseDTO>(createdOrder);
        }

        public async Task<OrderReportResponseDTO> UpdateOrderReportAsync(int orderReportId, OrderReportRequestDTO orderReportRequest)
        {
            var existingOrderReport = await _orderReportRepository.GetOrderReportByIdAsync(orderReportId);
            if (existingOrderReport == null)
            {
                throw new Exception($"Order Report with ID {orderReportId} not found.");
            }

            _mapper.Map(orderReportRequest, existingOrderReport);

            existingOrderReport.CreatedAt = DateTime.Now; 
            var updatedOrderReport = await _orderReportRepository.UpdateOrderReportAsync(existingOrderReport);

            return _mapper.Map<OrderReportResponseDTO>(updatedOrderReport);
        }

        public async Task<bool> DeleteOrderReportAsync(int orderReportId)
        {
            var existingOrderReport = await _orderReportRepository.GetOrderReportByIdAsync(orderReportId);
            if (existingOrderReport == null)
            {
                throw new Exception($"Order Report with ID {orderReportId} not found.");
            }

            return await _orderReportRepository.DeleteOrderReportAsync(orderReportId);
        }

        public async Task<OrderReportResponseDTO> GetOrderReportByIdAsync(int orderReportId)
        {
            var orderReport = await _orderReportRepository.GetOrderReportByIdAsync(orderReportId);
            if (orderReport == null)
            {
                throw new Exception($"Order Report with ID {orderReportId} not found.");
            }

            return _mapper.Map<OrderReportResponseDTO>(orderReport);
        }

    }
}
