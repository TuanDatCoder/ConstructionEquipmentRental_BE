using Microsoft.AspNetCore.Mvc;
using System.Net;
using Net.payOS;
using Net.payOS.Types;
using Data.DTOs;
using Services.PayOSServices;

namespace ConstructionEquipmentRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSController : ControllerBase
    {
        private readonly IPayOSService _payOSService;

        public PayOSController(IPayOSService payOSService)
        {
            _payOSService = payOSService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentLinkInformation(int orderCode)
        {
            try
            {
                var paymentLinkInformation = await _payOSService.GetPaymentLinkInformationAsync(orderCode);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Get payment link information successfully",
                    Data = paymentLinkInformation
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
    }
}
