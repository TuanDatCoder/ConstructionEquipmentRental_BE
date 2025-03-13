using Data.DTOs.Category;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ProductRepos
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts(int? page, int? size);
        Task<Product> GetByIdAsync(int id);
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(Product product);
        Task<List<Product>> GetProductsByStoreIdAsync(int storeId);
        Task<List<Product>> GetProductsByLessorIdAsync(int lessorId);
        Task<Category?> GetProductsByCategoryAsync(int categoryId);

    }
}
