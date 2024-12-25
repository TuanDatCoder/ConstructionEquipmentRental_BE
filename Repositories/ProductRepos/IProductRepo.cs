using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ProductRepos
{
    public interface IProductRepo
    {
        Task<List<Product>> GetProducts(int? page, int? size);
        Task<Product> GetByIdAsync(int id);
        Task Add(Product product);
        Task Update(Product product);

        Task Delete(Product product);
    }
}
