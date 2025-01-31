using Data.DTOs.PayOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PayOSServices
{
    public interface IPayOSService
    {
        Task<CreatePaymentResult> createPaymentUrl(PayOSRequestDTO payOSRequestDTO);
    }
}
