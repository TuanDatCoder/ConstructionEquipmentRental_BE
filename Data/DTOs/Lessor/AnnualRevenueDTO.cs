using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Lessor
{
    public class AnnualRevenueDTO
    {
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<MonthlyRevenueDTO> MonthlyRevenues { get; set; }
    }
}
