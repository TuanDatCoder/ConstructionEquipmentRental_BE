using Data.DTOs.OrderItem;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Order
{
    public class OrderWithItemsRequestDTO
    {
        public OrderForItemRequestDTO Order { get; set; }
        public List<ItemForOrderRequestDTO> OrderItems { get; set; }

    }
}
