using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.OrderItem
{
    public class ItemForOrderRequestDTO
    {

        public int ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
