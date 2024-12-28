using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ProductImageRepos
{
    public interface IProductImageRepository
    {
        Task<List<ProductImage>> GetProductImages(int? page, int? size);
        Task<ProductImage> GetByIdAsync(int id);
        Task Add(ProductImage productImage);
        Task Update(ProductImage productImage);
        Task Delete(ProductImage productImage);
    }
}
