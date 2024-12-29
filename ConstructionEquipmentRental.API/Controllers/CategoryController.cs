using Data.DTOs.Product;
using Data.DTOs;
using Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.ProductServices;
using System.Net;
using Services.CategoryServices;
using Data.Entities;
using Data.DTOs.Category;
using Data.Enums;

namespace ConstructionEquipmentRental.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int? page = 1, int? size = 10)
        {
            var result = await _categoryService.GetCategories(page, size);

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View categories successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Category retrieved successfully",
                    Data = category
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid category data"
                });
            }

            var result = await _categoryService.CreateCategory(request);

            return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Category created successfully",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDTO request)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateCategory(id, request);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Category updated successfully",
                    Data = updatedCategory
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Category deleted successfully"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeCategoryStatus(int id, [FromBody] CategoryStatusEnum newStatus)
        {
            try
            {
                var updatedCategory = await _categoryService.ChangeCategoryStatus(id, newStatus);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Category status updated successfully",
                    Data = updatedCategory
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }
    }
}
