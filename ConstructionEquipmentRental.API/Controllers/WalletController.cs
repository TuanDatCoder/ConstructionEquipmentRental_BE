using Data.DTOs.Transaction;
using Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.TransactionServices;
using System.Net;
using Services.WalletServices;
using Data.DTOs.Wallet;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallets(int? page = 1, int? size = 10)
        {
            var result = await _walletService.GetWallets(page, size);

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View Wallets successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletById(int id)
        {
            try
            {
                var transaction = await _walletService.GetWalletById(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Wallet retrieved successfully",
                    Data = transaction
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
        public async Task<IActionResult> CreateWallet([FromBody] WalletRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid Wallet data"
                });
            }

            var result = await _walletService.CreateWallet(request);

            return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Wallet created successfully",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] WalletRequestDTO request)
        {
            try
            {
                var updatedTransaction = await _walletService.UpdateWallet(id, request);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Wallet updated successfully",
                    Data = updatedTransaction
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
        public async Task<IActionResult> DeleteWallet(int id)
        {
            try
            {
                await _walletService.DeleteWallet(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Wallet deleted successfully"
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
