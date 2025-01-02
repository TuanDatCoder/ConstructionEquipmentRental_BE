using Data.DTOs.Wallet;
using Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.WalletServices;
using System.Net;
using Services.WalletLogServices;
using Data.DTOs.WalletLog;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletLogController : ControllerBase
    {
        private readonly IWalletLogService _walletLogService;

        public WalletLogController(IWalletLogService walletLogService)
        {
            _walletLogService = walletLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWalletLogs(int? page = 1, int? size = 10)
        {
            var result = await _walletLogService.GetWalletLogs(page, size);

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View WalletLog successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletLogById(int id)
        {
            try
            {
                var walletLog = await _walletLogService.GetWalletLogById(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "WalletLog retrieved successfully",
                    Data = walletLog
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
        public async Task<IActionResult> CreateWalletLog([FromBody] WalletLogRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid WalletLog data"
                });
            }

            var result = await _walletLogService.CreateWalletLog(request);

            return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "WalletLog created successfully",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWalletLog(int id, [FromBody] WalletLogRequestDTO request)
        {
            try
            {
                var updatedWalletLog = await _walletLogService.UpdateWalletLog(id, request);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "WalletLog updated successfully",
                    Data = updatedWalletLog
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
        public async Task<IActionResult> DeleteWalletLog(int id)
        {
            try
            {
                await _walletLogService.DeleteWalletLog(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "WalletLog deleted successfully"
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
