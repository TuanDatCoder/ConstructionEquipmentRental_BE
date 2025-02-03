using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Order
{
    public class OrderPaymentResponseDTO
    {
        public int orderId { get; set; }

        public string paymentUrl { get; set; }
    }
}
