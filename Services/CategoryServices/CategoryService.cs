using AutoMapper;
using Data.DTOs.Category;
using Data.DTOs.Product;
using Data.Entities;
using Data.Enums;
using Data.Models.Enums;
using Repositories.CategoryRepos;
using Repositories.ProductRepos;
using Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryResponseDTO>> GetCategories(int? page, int? size)
        {
            var categories = await _categoryRepository.GetCategories(page, size);
            return _mapper.Map<List<CategoryResponseDTO>>(categories);
        }


        public async Task<CategoryResponseDTO> GetCategoryById(int id)
        {

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found.");
            }

            return _mapper.Map<CategoryResponseDTO>(category);
        }


        public async Task<CategoryResponseDTO> CreateCategory(CategoryRequestDTO request)
        {
            var category = _mapper.Map<Category>(request);
            category.Status = CategoryStatusEnum.ACTIVE.ToString();
            await _categoryRepository.Add(category);
            return _mapper.Map<CategoryResponseDTO>(category);
        }

        public async Task<CategoryResponseDTO> UpdateCategory(int id, CategoryRequestDTO request)
        {

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found.");
            }

            _mapper.Map(request, category);


            await _categoryRepository.Update(category);

            return _mapper.Map<CategoryResponseDTO>(category);
        }
        public async Task DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new Exception($"Category with ID {id} not found.");
            }

            await _categoryRepository.Delete(category);
        }

        public async Task<CategoryResponseDTO> ChangeCategoryStatus(int id, CategoryStatusEnum newStatus)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }


            existingCategory.Status = newStatus.ToString();

            await _categoryRepository.Update(existingCategory);

            return _mapper.Map<CategoryResponseDTO>(existingCategory);
        }



    }
}
