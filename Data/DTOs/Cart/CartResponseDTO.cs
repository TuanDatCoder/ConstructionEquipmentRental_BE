using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Cart
{
    public class CartResponseDTO
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? Status { get; set; }
        public string? AccountName { get; set; }
    }
}
