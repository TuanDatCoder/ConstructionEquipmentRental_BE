using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Order
{
    public class OrderRequestDTO
    {
        public int StaffId { get; set; }

        public int CustomerId { get; set; }

        public decimal? TotalPrice { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public string? PurchaseMethod { get; set; }

        public string? RecipientName { get; set; }

        public string? RecipientPhone { get; set; }

        public string? Address { get; set; }

        public DateOnly? DateOfReceipt { get; set; }

        public DateOnly? DateOfReturn { get; set; }
    }
}
