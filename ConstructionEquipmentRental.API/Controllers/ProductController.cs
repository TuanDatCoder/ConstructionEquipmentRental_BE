using BuildLease.Data.DTOs.Product;
using Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.ProductServices;
using System.Net;

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
        public async Task<IActionResult> GetProducts(int? page = 1, int? size = 10)
        {
            var result = await _productService.GetProducts(page, size);

            var response = new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View products successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid product data"
                });
            }

            var result = await _productService.CreateProduct(request);

            var response = new
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Product created successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequestDTO request)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProduct(id, request);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return NoContent(); // 204 No Content: Sản phẩm đã được xóa
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message }); // 404 Not Found
            }
        }
    }
}
