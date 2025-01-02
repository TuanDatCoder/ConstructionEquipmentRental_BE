using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.WalletLog
{
    public class WalletLogResponseDTO
    {
        public int Id { get; set; }

        public int WalletId { get; set; }

        public int? TransactionId { get; set; }

        public string Type { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = null!;
    }
}
