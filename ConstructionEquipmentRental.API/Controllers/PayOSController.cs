using Microsoft.AspNetCore.Mvc;
using System.Net;
using Net.payOS;
using Net.payOS.Types;
using Data.DTOs;

namespace ConstructionEquipmentRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PayOSController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentLinkInformation(int orderCode)
        {
            PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);

            PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(orderCode);

            ApiResponseDTO response = new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get payment link information successfully",
                Data = paymentLinkInformation
            };

            return StatusCode(response.Code, response);
        }
    }
}
