using Data.DTOs.Cart;
using Data.DTOs.Product;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartResponseDTO>> GetCarts(int? page, int? size);
        Task<CartResponseDTO> GetCartById(int id);
        Task<CartResponseDTO> CreateCart(CartRequestDTO request);
        Task<CartResponseDTO> UpdateCart(int id, CartUpdateRequestDTO request);
        Task DeleteCart(int id);
        Task<CartResponseDTO> ChangeCartStatus(int id, CartStatusEnum newStatus);
    }
}
