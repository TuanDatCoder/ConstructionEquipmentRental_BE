using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Wallet
{
    public class WalletResponseDTO
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string? BankName { get; set; }

        public string? BankAccount { get; set; }

        public decimal Balance { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
