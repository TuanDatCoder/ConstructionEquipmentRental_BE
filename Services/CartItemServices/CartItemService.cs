using AutoMapper;
using Data.DTOs.Cart;
using Data.DTOs.CartItem;
using Data.Entities;
using Data.Enums;
using Repositories.CartItemRepos;
using Repositories.CartRepos;
using Services.CartServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CartItemServices
{
    public class CartItemService : ICartItemService
    {
        private readonly IMapper _mapper;
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(IMapper mapper, ICartItemRepository cartItemRepository)
        {
            _mapper = mapper;
            _cartItemRepository = cartItemRepository;
        }
        public async Task<List<CartItemResponseDTO>> GetCartItems(int? page, int? size)
        {
            var cartItems = await _cartItemRepository.GetCartItems(page, size);
            return _mapper.Map<List<CartItemResponseDTO>>(cartItems);
        }


        public async Task<CartItemResponseDTO> GetCartItemById(int id)
        {

            var cart = await _cartItemRepository.GetByIdAsync(id);

            if (cart == null)
            {
                throw new Exception($"CartItem with ID {id} not found.");
            }

            return _mapper.Map<CartItemResponseDTO>(cart);
        }


        public async Task<CartItemResponseDTO> CreateCartItem(CartItemRequestDTO request)
        {
            var cartItem = _mapper.Map<CartItem>(request);

            await _cartItemRepository.Add(cartItem);
            return _mapper.Map<CartItemResponseDTO>(cartItem);
        }

        public async Task<CartItemResponseDTO> UpdateCartItem(int id, CartItemRequestDTO request)
        {

            var cart = await _cartItemRepository.GetByIdAsync(id);

            if (cart == null)
            {
                throw new Exception($"CartItem with ID {id} not found.");
            }

            _mapper.Map(request, cart);


            await _cartItemRepository.Update(cart);

            return _mapper.Map<CartItemResponseDTO>(cart);
        }
        public async Task DeleteCartItem(int id)
        {
            var cart = await _cartItemRepository.GetByIdAsync(id);

            if (cart == null)
            {
                throw new Exception($"CartItem with ID {id} not found.");
            }

            await _cartItemRepository.Delete(cart);
        }

       



    }
}
