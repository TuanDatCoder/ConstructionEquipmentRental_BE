using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.OrderItem
{
    public class OrderItemRequestDTO
    {
        
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}
