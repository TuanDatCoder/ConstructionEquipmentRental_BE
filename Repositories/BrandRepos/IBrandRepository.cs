using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.BrandRepos
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetBrands(int? page, int? size);
        Task<Brand> GetByIdAsync(int id);
        Task Add(Brand brand);
        Task Update(Brand brand);
        Task Delete(Brand brand);
    }
}
