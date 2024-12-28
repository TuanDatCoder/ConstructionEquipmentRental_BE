using Data.DTOs.Category;
using Data.DTOs.Product;
using Data.Enums;
using Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDTO>> GetCategories(int? page, int? size);
        Task<CategoryResponseDTO> GetCategoryById(int id);
        Task<CategoryResponseDTO> CreateCategory(CategoryRequestDTO request);
        Task<CategoryResponseDTO> UpdateCategory(int id, CategoryRequestDTO request);
        Task DeleteCategory(int id);
        Task<CategoryResponseDTO> ChangeCategoryStatus(int id, CategoryStatusEnum newStatus);
    }
}
