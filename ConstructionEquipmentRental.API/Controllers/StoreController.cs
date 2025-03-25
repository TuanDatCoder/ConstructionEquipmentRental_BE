using Data.DTOs.Product;
using Data.DTOs;
using Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.ProductServices;
using System.Net;
using Services.StoreServices;
using Data.DTOs.Store;
using Microsoft.AspNetCore.Authorization;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStores()
        {
            var result = await _storeService.GetStores();

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View stores successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(int id)
        {
            try
            {
                var product = await _storeService.GetStoreById(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Store retrieved successfully",
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
        public async Task<IActionResult> CreateStore([FromForm] StoreRequestDTO request, IFormFile? file = null)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid store data"
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

                var result = await _storeService.CreateStore(token, request, fileStream, fileName);

                return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.Created,
                    Message = "Store created successfully",
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
        public async Task<IActionResult> UpdateStore(int id, [FromBody] StoreUpdateRequestDTO request)
        {
            try
            {
                var updatedStore = await _storeService.UpdateStore(id, request);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Store updated successfully",
                    Data = updatedStore
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
        public async Task<IActionResult> DeleteStore(int id)
        {
            try
            {
                await _storeService.DeleteStore(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Store deleted successfully"
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
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ChangeStoreStatus(int id, [FromBody] StoreStatusEnum newStatus)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            try
            {
                var updatedProduct = await _storeService.ChangeStoreStatus(token, id, newStatus);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Store status updated successfully",
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

        [HttpGet("status")]
        public async Task<IActionResult> GetStoresByStatus([FromQuery] StoreStatusEnum status)
        {
            try
            {
                var stores = await _storeService.GetStoresByStatus(status);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lấy danh sách cửa hàng thành công",
                    Data = stores
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
        [HttpGet("by-lessor")]
        [Authorize(Roles = "LESSOR")]
        public async Task<IActionResult> GetStoresByLessor()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            try
            {
                var store = await _storeService.GetStoresByLessor(token);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = $"Store for Lessor retrieved successfully",
                    Data = store
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
