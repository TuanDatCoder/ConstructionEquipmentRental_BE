using Data.Entities;
using Data;
using Repositories.CartRepos;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories.CartItemRepos
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public CartItemRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<CartItem>> GetCartItems(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.CartItems
                    .Include(p => p.Cart)
                    .Include(p => p.Product)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            return await _context.CartItems
                 .Include(p => p.Cart)
                    .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task Add(CartItem cartItem)
        {
            try
            {
                await _context.CartItems.AddAsync(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when adding cartItem: {ex.Message}");
            }
        }

        public async Task Update(CartItem cartItem)
        {
            try
            {
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating cartItem: {ex.Message}");
            }
        }

        public async Task Delete(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        public async Task<List<CartItem>> GetCartItemsByCartId(int cartId)
        {
            try
            {
                return await _context.CartItems
                    .Where(ci => ci.CartId == cartId)
                    .Include(ci => ci.Product)
                    .Include(ci => ci.Cart)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when fetching CartItems by CartId: {ex.Message}");
            }
        }


    }
}
