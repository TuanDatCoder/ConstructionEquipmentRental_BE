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
using Repositories.TransactionRepos;
using System.Transactions;

namespace Services.PayOSServices
{
    public class PayOSService : IPayOSService
    {
        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionRepository _transactionRepository;
        public PayOSService(IConfiguration config, IOrderRepository orderRepository, ITransactionRepository transactionRepository)
        {
            _config = config;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<PaymentLinkInformation> GetPaymentLinkInformationAsync(int orderCode)
        {
        
            var currOrder = await _orderRepository.GetOrderByIdAsync(orderCode);
            if (currOrder == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Order does not exist");
            }


            if (!currOrder.PaymentMethod.Equals("PAYOS"))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "This transaction is not paid with PAYOS");
            }

            PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);
            PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(orderCode);

            string status = paymentLinkInformation.status.Equals("PAID")
                ? OrderStatusEnum.COMPLETED.ToString()
                : paymentLinkInformation.status;

            if (!status.Equals(currOrder.Status))
            {
                currOrder.UpdatedAt = DateTime.UtcNow;
                currOrder.Status = status;
                await _orderRepository.UpdateOrderAsync(currOrder);
            }
 

            var currTransaction = await _transactionRepository.GetByOrderIdAsync(orderCode);
            if (currTransaction != null && (!status.Equals(currTransaction.Status)))
            {
                currTransaction.Status = status;
                await _transactionRepository.Update(currTransaction);
            }

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
