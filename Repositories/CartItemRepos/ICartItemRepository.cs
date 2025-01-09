using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CartItemRepos
{
    public interface ICartItemRepository
    {
        Task<List<CartItem>> GetCartItems(int? page, int? size);
        Task<CartItem> GetByIdAsync(int id);
        Task Add(CartItem cartItem);
        Task Update(CartItem cartItem);

        Task Delete(CartItem cartItem);
    }
}
