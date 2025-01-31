using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Order
{
    public class OrderPaymentRequestDTO
    {

        [Required(ErrorMessage = "Order id is required")]
        public int orderId { get; set; }
        [Required(ErrorMessage = "RedirectUrl id is required")]
        public string redirectUrl { get; set; }
    }
}
