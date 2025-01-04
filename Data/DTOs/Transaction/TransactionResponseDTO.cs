using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Transaction
{
    public class TransactionResponseDTO
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public int AccountId { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

    }
}
