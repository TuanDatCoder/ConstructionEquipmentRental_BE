using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Cart
{
    public class CartUpdateRequestDTO
    {

        public int AccountId { get; set; }

        public CartStatusEnum Status { get; set; }
    }
}
