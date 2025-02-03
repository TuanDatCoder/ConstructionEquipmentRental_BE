using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Order
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string? Status { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PurchaseMethod { get; set; }

        public string? RecipientName { get; set; }

        public string? RecipientPhone { get; set; }

        public string? Address { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateOnly? DateOfReceipt { get; set; }

        public DateOnly? DateOfReturn { get; set; }
        public string? PayOsUrl { get; set; }
    }
}
