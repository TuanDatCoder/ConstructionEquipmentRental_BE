using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using Net.payOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DTOs.PayOS;

namespace Services.PayOSServices
{
    public class PayOSService : IPayOSService
    {
        private readonly IConfiguration _config;

        public PayOSService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<CreatePaymentResult> createPaymentUrl(PayOSRequestDTO payOSRequestDTO)
        {
            PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);

            ItemData item = new ItemData("Chuyển Khoản cho Tuấn Đạt", 1, (int)payOSRequestDTO.Amount);
            List<ItemData> items = new List<ItemData>();
            items.Add(item);

            PaymentData paymentData = new PaymentData(payOSRequestDTO.OrderId, (int)payOSRequestDTO.Amount, "Thanh toán subscription", items, payOSRequestDTO.CancelUrl, payOSRequestDTO.RedirectUrl);

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);

            return createPayment;
        }
    }
}
