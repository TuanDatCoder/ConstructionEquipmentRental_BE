using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CategoryRepos
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories(int? page, int? size);
        Task<Category> GetByIdAsync(int id);
        Task Add(Category category);
        Task Update(Category category);
        Task Delete(Category category);
    }
}
