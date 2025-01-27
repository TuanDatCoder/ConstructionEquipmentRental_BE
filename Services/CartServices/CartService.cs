using AutoMapper;
using Data.DTOs.Cart;
using Data.DTOs.Category;
using Data.Entities;
using Data.Enums;
using Repositories.CartRepos;
using Repositories.CategoryRepos;
using Services.CategoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;

        public CartService(IMapper mapper, ICartRepository cartRepository)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
        }
        public async Task<List<CartResponseDTO>> GetCarts(int? page, int? size)
        {
            var carts = await _cartRepository.GetCarts(page, size);
            return _mapper.Map<List<CartResponseDTO>>(carts);
        }


        public async Task<CartResponseDTO> GetCartById(int id)
        {

            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                throw new Exception($"Cart with ID {id} not found.");
            }

            return _mapper.Map<CartResponseDTO>(cart);
        }


        public async Task<CartResponseDTO> CreateCart(CartRequestDTO request)
        {
            var cart = _mapper.Map<Cart>(request);
            cart.Status = CartStatusEnum.ACTIVE.ToString();
            cart.CreatedAt = DateTime.Now;
            cart.UpdatedAt = DateTime.Now;
            await _cartRepository.Add(cart);
            return _mapper.Map<CartResponseDTO>(cart);
        }

        public async Task<CartResponseDTO> UpdateCart(int id, CartUpdateRequestDTO request)
        {

            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                throw new Exception($"Cart with ID {id} not found.");
            }

            cart.UpdatedAt = DateTime.Now;
            _mapper.Map(request, cart);
           

            await _cartRepository.Update(cart);

            return _mapper.Map<CartResponseDTO>(cart);
        }
        public async Task DeleteCart(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                throw new Exception($"Cart with ID {id} not found.");
            }

            await _cartRepository.Delete(cart);
        }

        public async Task<CartResponseDTO> ChangeCartStatus(int id, CartStatusEnum newStatus)
        {
            var existingCart = await _cartRepository.GetByIdAsync(id);
            if (existingCart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {id} not found.");
            }


            existingCart.Status = newStatus.ToString();

            await _cartRepository.Update(existingCart);

            return _mapper.Map<CartResponseDTO>(existingCart);
        }



    }
}
