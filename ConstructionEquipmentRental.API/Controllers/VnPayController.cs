using Data.DTOs.VnPay;
using Microsoft.AspNetCore.Mvc;
using Services.VnPayServices;

namespace ConstructionEquipmentRental.API.Controllers
{
    [Route("api/vnpay")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        /// <summary>
        /// Tạo URL thanh toán VNPAY
        /// </summary>
        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] VnPayPaymentInformationDTO request)
        {
            if (request == null || request.Amount <= 0)
            {
                return BadRequest("Thông tin đơn hàng không hợp lệ.");
            }

            var paymentUrl = _vnPayService.CreatePaymentUrl(request, HttpContext);
            return Ok(new { Url = paymentUrl });
        }

        /// <summary>
        /// Xử lý kết quả thanh toán từ VNPAY
        /// </summary>
        [HttpGet("callback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                return Ok(new { Message = "Thanh toán thành công!", Data = response });
            }

            return BadRequest(new { Message = "Thanh toán thất bại!", Data = response });
        }
    }
}
