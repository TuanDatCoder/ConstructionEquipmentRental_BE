using Data.DTOs.Lessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LessorServices
{
    public interface ILessorService
    {
        Task<List<AnnualRevenueDTO>> GetRevenueByLessorAsync(int lessorId);
    }
}
