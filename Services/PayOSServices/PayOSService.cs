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
using Data.Entities;
using Repositories.OrderItemRepos;
using Repositories.ProductRepos;

namespace Services.PayOSServices
{
    public class PayOSService : IPayOSService
    {
        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        public PayOSService(IConfiguration config, IOrderRepository orderRepository, ITransactionRepository transactionRepository, IOrderItemRepository orderItemRepository, IProductRepository productRepository)
        {
            _config = config;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
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
            if(paymentLinkInformation.status.Equals("CANCELLED") && currOrder.Status.Equals("PENDING"))
            {
                var orderItems = await _orderItemRepository.GetOrderItemsByOrderIdAsync(orderCode);
                foreach (var item in orderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new ApiException(HttpStatusCode.NotFound, "Product does not exist");
                    }
                    product.Stock += item.Quantity; // trả lại số hàng
                    await _productRepository.Update(product);
                    item.Status = "CANCELLED";
                    await _orderItemRepository.UpdateOrderItemAsync(item);

                }
            }
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

            var currOrder = await _orderRepository.GetOrderByIdAsync(payOSRequestDTO.OrderId);
            if (currOrder == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Order does not exist");
            }


            List<ItemData> items = new List<ItemData>();

            foreach (var orderItem in payOSRequestDTO.OrderItems)
            {
              
                int quantity = orderItem.Quantity; 
                int price = (int)(orderItem.Price);
                string productName = orderItem.ProductName ?? "Unknown Product";
                items.Add(new ItemData(productName, quantity, price));
            }

            // chèn vô tính tổng tiền theo ngày thuê
            var rentalDays = (DateTime.Parse(currOrder.DateOfReturn.ToString()) - DateTime.Parse(currOrder.DateOfReceipt.ToString())).Days;

            items.Add(new ItemData("Tổng số ngày thuê", rentalDays, (int)payOSRequestDTO.Amount/ rentalDays));

            PaymentData paymentData = new PaymentData(
                payOSRequestDTO.OrderId,
                (int)payOSRequestDTO.Amount,
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
