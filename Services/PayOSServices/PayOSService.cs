using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using Net.payOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DTOs.PayOS;
using Services.OrderServices;
using Repositories.OrderRepos;
using Data.Enums;
using Services.Helper.CustomExceptions;
using System.Net;
using Services.TransactionServices;
using Data.DTOs.Transaction;

namespace Services.PayOSServices
{
    public class PayOSService : IPayOSService
    {
        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionService _transactionService;
        public PayOSService(IConfiguration config, IOrderRepository orderRepository)
        {
            _config = config;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentLinkInformation> GetPaymentLinkInformationAsync(int orderCode)
        {
            PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);

            PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(orderCode);

            var currOrder = await _orderRepository.GetOrderByIdAsync(orderCode);

            if (currOrder == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Order does not exist");
            }

            currOrder.Status = paymentLinkInformation.status;
            if (currOrder.Status.Equals("PAID"))
            {
                currOrder.Status = OrderStatusEnum.COMPLETED.ToString();
                currOrder.PaymentMethod = PaymentMethodEnum.PAYOS.ToString();
            }
            await _orderRepository.UpdateOrderAsync(currOrder);

            var transactionRequestDTO = new TransactionRequestDTO
            {
                OrderId = orderCode,
                AccountId = currOrder.CustomerId,
                PaymentMethod = currOrder.PaymentMethod,
                TotalPrice = paymentLinkInformation.amountPaid,
                Status = currOrder.Status
            };

            await _transactionService.CreateTransaction(transactionRequestDTO);

            return paymentLinkInformation;
        }
      

        public async Task<CreatePaymentResult> createPaymentUrl(PayOSRequestDTO payOSRequestDTO)
        {
            PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);

            List<ItemData> items = new List<ItemData>();

            foreach (var orderItem in payOSRequestDTO.OrderItems)
            {
              
                int quantity = orderItem.Quantity ?? 1; 
                int price = (int)(orderItem.Price ?? 0);
                string productName = orderItem.ProductName ?? "Unknown Product";
                items.Add(new ItemData(productName, quantity, price));
            }

            int totalAmount = items.Sum(i => i.price * i.quantity);

            PaymentData paymentData = new PaymentData(
                payOSRequestDTO.OrderId,
                totalAmount,
                "Thanh toán đơn hàng",
                items,
                payOSRequestDTO.CancelUrl,
                payOSRequestDTO.RedirectUrl
            );

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);

            return createPayment;
        }

    }
}
