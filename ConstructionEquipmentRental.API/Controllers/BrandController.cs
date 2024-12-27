using Data.DTOs.Brand;
using Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.BrandServices;
using System.Net;
using System.Threading.Tasks;
using BuildLease.Data.Models.Enums;
using Data.Enums;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands([FromQuery] int? page, [FromQuery] int? size)
        {
            var result = await _brandService.GetAllBrandsAsync(page, size);
            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Brands retrieved successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var result = await _brandService.GetBrandByIdAsync(id);
            if (result == null)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Brand not found",
                    Data = null
                });
            }

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Brand retrieved successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] BrandRequestDTO brandRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid brand data",
                    Data = null
                });
            }

            var result = await _brandService.CreateBrandAsync(brandRequest);
            return CreatedAtAction(nameof(GetBrandById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandRequestDTO brandRequest)
        {
            var result = await _brandService.UpdateBrandAsync(id, brandRequest);
            if (result == null)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Brand not found",
                    Data = null
                });
            }

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Brand updated successfully",
                Data = result
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var isDeleted = await _brandService.DeleteBrandAsync(id);
            if (!isDeleted)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Brand not found",
                    Data = null
                });
            }

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Brand deleted successfully",
                Data = null
            });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeBrandStatus(int id, [FromBody] BrandStatusEnum newStatus)
        {
            try
            {
                var updated = await _brandService.ChangeBrandStatus(id, newStatus);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Brand status updated successfully",
                    Data = updated
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
