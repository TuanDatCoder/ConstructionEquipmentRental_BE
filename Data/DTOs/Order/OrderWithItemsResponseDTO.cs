using Data.DTOs.OrderItem;
using Data.DTOs.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.DTOs.Order
{
    public class OrderWithItemsResponseDTO
    {
        public OrderResponseDTO Order { get; set; }
        public List<OrderItemResponseDTO> OrderItems { get; set; }
        public TransactionResponseDTO Transaction { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PayOsUrl { get; set; }
    }
}
