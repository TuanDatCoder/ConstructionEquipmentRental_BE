using Data.DTOs.Product;
using Data.DTOs;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.ProductServices;
using System.Net;
using Services.ProductImageServices;
using Data.DTOs.ProductImage;

namespace ConstructionEquipmentRental.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductImages(int? page = 1, int? size = 10)
        {
            var result = await _productImageService.GetProductImages(page, size);

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View productImages successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductImageById(int id)
        {
            try
            {
                var product = await _productImageService.GetProductImageById(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "ProductImage retrieved successfully",
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
        public async Task<IActionResult> CreateProductImage([FromBody] ProductImageRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid productImage data"
                });
            }

            var result = await _productImageService.CreateProductImage(request);

            return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "ProductImage created successfully",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(int id, [FromBody] ProductImageRequestDTO request)
        {
            try
            {
                var updatedProductImage = await _productImageService.UpdateProductImage(id, request);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Product updated successfully",
                    Data = updatedProductImage
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
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            try
            {
                await _productImageService.DeleteProductImage(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "ProductImage deleted successfully"
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

        //[HttpPatch("{id}/status")]
        //public async Task<IActionResult> ChangeProductImageStatus(int id, [FromBody] ProductImageStatusEnum newStatus)
        //{
        //    try
        //    {
        //        var updatedProductImage = await _productService.ChangeProductImageStatus(id, newStatus);

        //        return Ok(new ApiResponseDTO
        //        {
        //            IsSuccess = true,
        //            Code = (int)HttpStatusCode.OK,
        //            Message = "ProductImage status updated successfully",
        //            Data = updatedProductImage
        //        });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new ApiResponseDTO
        //        {
        //            IsSuccess = false,
        //            Code = (int)HttpStatusCode.NotFound,
        //            Message = ex.Message
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponseDTO
        //        {
        //            IsSuccess = false,
        //            Code = (int)HttpStatusCode.BadRequest,
        //            Message = ex.Message
        //        });
        //    }
        //}
    }
}
