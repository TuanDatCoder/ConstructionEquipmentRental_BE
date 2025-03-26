using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Lessor
{
    public class LessorSummaryDTO
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalEquipment { get; set; }
        public double? TotalOrdersPercentageChange { get; set; }
        public double? TotalRevenuePercentageChange { get; set; }
        public double? TotalEquipmentPercentageChange { get; set; }
    }
}
