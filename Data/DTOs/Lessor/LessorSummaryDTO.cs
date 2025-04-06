using System;

namespace Data.DTOs.Lessor
{
    public class LessorSummaryDTO
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalEquipment { get; set; }
        public int TotalRentedEquipment { get; set; } // Tổng thiết bị đang cho thuê
        public double? TotalOrdersPercentageChange { get; set; }
        public double? TotalRevenuePercentageChange { get; set; }
        public double? TotalEquipmentPercentageChange { get; set; }
        public double? TotalRentedEquipmentPercentageChange { get; set; } // % thay đổi của thiết bị đang cho thuê
    }
}