using Data.DTOs.VnPay;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.VnPayServices
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(VnPayPaymentInformationDTO model, HttpContext context);
        VnPayPaymentResponseDTO PaymentExecute(IQueryCollection collections);


    }
}
