using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.PayOS
{
    public class PayOSRequestDTO
    {
        public int OrderId { get; set; }
        public int PaymentId { get; set; }

        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RedirectUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
