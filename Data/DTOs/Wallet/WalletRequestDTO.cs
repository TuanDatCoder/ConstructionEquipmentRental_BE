using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Wallet
{
    public class WalletRequestDTO
    {

        public int AccountId { get; set; }

        public string? BankName { get; set; }

        public string? BankAccount { get; set; }

        public decimal Balance { get; set; }

        public string Status { get; set; } = null!;
    }
}
