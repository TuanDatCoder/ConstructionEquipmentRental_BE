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
    }
}
