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
    }
}
