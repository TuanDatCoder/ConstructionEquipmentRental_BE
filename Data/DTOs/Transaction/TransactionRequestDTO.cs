using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Transaction
{
    public class TransactionRequestDTO
    {
        public int? OrderId { get; set; }

        public int AccountId { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = null!;
    }
}
