using Data.DTOs.Product;
using Data.Enums;
using Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.ProductServices;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Data.Entities;
using Newtonsoft.Json.Linq;
using Google.Apis.Auth.OAuth2.Requests;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int? page = 1, int? size = 100)
        {
            var result = await _productService.GetProducts(page, size);

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View products successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Product retrieved successfully",
                    Data = product
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
        [Authorize(Roles = "LESSOR")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequestDTO request, IFormFile? file = null)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid product data"
                });
            }

            try
            {
                Stream? fileStream = null;
                string? fileName = null;

                if (file != null && file.Length > 0)
                {
                    fileStream = file.OpenReadStream();
                    fileName = file.FileName;
                }

                var result = await _productService.CreateProductAsync(token, request, fileStream, fileName);

                return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.Created,
                    Message = "Product created successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }



        [HttpPut("{id}")]
        [Authorize(Roles = "LESSOR")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateRequestDTO request, IFormFile? file)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponseDTO
                    {
                        IsSuccess = false,
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Invalid product data"
                    });
                }

                using var fileStream = file?.OpenReadStream();
                var result = await _productService.UpdateProduct(id, token, request, fileStream, file?.FileName);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Product updated successfully",
                    Data = result
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
        [Authorize(Roles = "LESSOR,ADMIN")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Product deleted successfully"
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
        public async Task<IActionResult> ChangeProductStatus(int id, [FromBody] ProductStatusEnum newStatus)
        {
            try
            {
                var updatedProduct = await _productService.ChangeProductStatus(id, newStatus);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Product status updated successfully",
                    Data = updatedProduct
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


        [HttpPost("/{productId}/upload-picture")]
        [Authorize(Roles = "LESSOR")]
        public async Task<IActionResult> UploadPicture(int productId, IFormFile file)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "File is not selected"
                });
            }

            try
            {
                using var fileStream = file.OpenReadStream();
                string fileUrl = await _productService.UpdatePictureAsync(productId, token, fileStream, file.FileName);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Avatar uploaded successfully",
                    Data = new { FileUrl = fileUrl }
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,  new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("by-store/{storeId}")]
        public async Task<IActionResult> GetProductsByStoreId(int storeId)
        {
            try
            {
                var prooducts = await _productService.GetProductsByProductIdAsync(storeId);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = $"Products for Store ID {storeId} retrieved successfully",
                    Data = prooducts
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

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var result = await _productService.GetProductsByCategoryAsync(categoryId);

                if (result == null)
                {
                    return NotFound(new ApiResponseDTO
                    {
                        IsSuccess = false,
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "No category found or no products available."
                    });
                }

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = $"Products for Category ID {categoryId} retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }
        [HttpGet("categories-products")]
        public async Task<IActionResult> GetCategoriesAndTotalProduct(int? page = 1, int? size = 100)
        {
            try
            {
                var result = await _productService.GetCategoriesAndTotalProductAsync(page,size );

                if (result == null)
                {
                    return NotFound(new ApiResponseDTO
                    {
                        IsSuccess = false,
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "No category found or no products available."
                    });
                }

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = $"Products for Categories retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("categories-with-products")]
        public async Task<IActionResult> GetCategoriesAndTotalProductAsync(int? page, int? size)
        {
            try
            {
                // Gọi service để lấy danh sách categories và sản phẩm
                var result = await _productService.GetCategoriesAndTotalProductAsync(page, size);

                // Kiểm tra nếu kết quả rỗng
                if (result == null || !result.Any())
                {
                    return NotFound(new ApiResponseDTO
                    {
                        IsSuccess = false,
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "No categories found."
                    });
                }

                // Trả về kết quả thành công
                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Categories with products retrieved successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }


    }
}
