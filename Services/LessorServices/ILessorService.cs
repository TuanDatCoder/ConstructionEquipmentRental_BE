using Data.DTOs.Lessor;
using Data.DTOs.Order;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LessorServices
{
    public interface ILessorService
    {
        Task<List<AnnualRevenueDTO>> GetRevenueByLessorAsync(string token );
        Task<List<OrderAndItemsResponseDTO>> GetOrdersByLessorAsync(string token);
        public Task<LessorSummaryDTO> GetLessorSummaryAsync(string token);
    }
}
