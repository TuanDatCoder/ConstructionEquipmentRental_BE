﻿using Data.DTOs.VnPay;
using Microsoft.AspNetCore.Mvc;
using Services.VnPayServices;

namespace ConstructionEquipmentRental.API.Controllers
{
   
   
    public class PaymentController : Controller
    {

        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrlVnpay(VnPayPaymentInformationDTO model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }


    }


}
