using Data.DTOs;
using Data.DTOs.ProductImage;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ProductImageServices;
using System.Net;

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
        public async Task<IActionResult> UpdateProductImage(int id, [FromBody] ProductImageUpdateRequestDTO request)
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

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeProductImageStatus(int id, [FromBody] ProductImageStatusEnum newStatus)
        {
            try
            {
                var updatedProductImage = await _productImageService.ChangeProductImageStatus(id, newStatus);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "ProductImage status updated successfully",
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
        [HttpPost("upload-multiple")]
        [Authorize(Roles = "LESSOR")]
        public async Task<IActionResult> UploadMultipleProductImages(int productId, [FromForm] List<IFormFile> files)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid request data"
                });
            }

            if (files == null || files.Count == 0)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "No files were uploaded."
                });
            }

            try
            {
                var fileStreams = files.Select(file => file.OpenReadStream()).ToList();
                var fileNames = files.Select(file => file.FileName).ToList();
                var productImages = await _productImageService.UploadMultipleProductImagesAsync(token,fileStreams, fileNames, productId);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Files uploaded successfully",
                    Data = productImages
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = "An error occurred while uploading files. Please try again later."
                });
            }
        }


        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetProductImagesByProductId(int productId)
        {
            try
            {
                var productImages = await _productImageService.GetProductImagesByProductId(productId);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Product images retrieved successfully",
                    Data = productImages
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
