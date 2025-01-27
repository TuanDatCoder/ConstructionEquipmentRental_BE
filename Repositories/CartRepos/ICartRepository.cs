using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CartRepos
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetCarts(int? page, int? size);
        Task<Cart> GetByIdAsync(int id);
        Task Add(Cart cart);
        Task Update(Cart cart);

        Task Delete(Cart cart);
    }
}
