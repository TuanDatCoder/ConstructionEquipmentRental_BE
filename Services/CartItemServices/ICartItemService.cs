using Data.DTOs.Cart;
using Data.DTOs.CartItem;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CartItemServices
{
    public interface ICartItemService
    {
        Task<List<CartItemResponseDTO>> GetCartItems(int? page, int? size);
        Task<CartItemResponseDTO> GetCartItemById(int id);
        Task<CartItemResponseDTO> CreateCartItem(CartItemRequestDTO request);
        Task<CartItemResponseDTO> UpdateCartItem(int id, CartItemRequestDTO request);
        Task DeleteCartItem(int id);
        
    }
}
