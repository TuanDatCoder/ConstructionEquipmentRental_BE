using AutoMapper;
using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using Data.DTOs.PayOS;
using Data.DTOs.Transaction;
using Data.Entities;
using Data.Enums;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;
using Repositories.OrderRepos;
using Services.AuthenticationServices;
using Services.Helper.CustomExceptions;
using Services.OrderItemServices;
using Services.PayOSServices;
using Services.ProductServices;
using Services.TransactionServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductService _productService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly IPayOSService _payOSService;
        private readonly IAuthenticationService _authenticationService;

        public OrderService(IOrderRepository orderRepository, IOrderItemService orderItemService, IMapper mapper, IPayOSService payOSService, IProductService productService, ITransactionService transactionService, IAuthenticationService authenticationService)
        {
            _orderRepository = orderRepository;
            _orderItemService = orderItemService;
            _productService = productService;
            _transactionService = transactionService;
            _mapper = mapper;
            _payOSService= payOSService;
            _authenticationService= authenticationService;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(OrderRequestDTO orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);
            order.Status = "CONFIRMED";
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            return _mapper.Map<OrderResponseDTO>(createdOrder);
        }

        public async Task<OrderResponseDTO> UpdateOrderAsync(int orderId, OrderRequestDTO orderRequest)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            _mapper.Map(orderRequest, existingOrder);
            existingOrder.CreatedAt = existingOrder.CreatedAt;
            existingOrder.UpdatedAt = DateTime.Now;
            var updatedOrder = await _orderRepository.UpdateOrderAsync(existingOrder);

            return _mapper.Map<OrderResponseDTO>(updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }
            return await _orderRepository.DeleteOrderAsync(orderId);
        }

        public async Task<OrderResponseDTO> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            return _mapper.Map<OrderResponseDTO>(order);
        }


      
        public async Task<string> GetPaymentUrl(HttpContext context, int orderId, string redirectUrl)
        {
            var currOrder = await _orderRepository.GetOrderByIdAsync(orderId);

            if (currOrder == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Order does not exist");
            }

            if (currOrder.Status.Equals(OrderStatusEnum.COMPLETED.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The order has already been completed");
            }

            //nếu có link thì trả về luôn
            if (!string.IsNullOrEmpty(currOrder.PayOsUrl)) {
                var link = currOrder.PayOsUrl;
                return link;
            }

            var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);

            PayOSRequestDTO payOSRequestDTO = new PayOSRequestDTO
            {
                OrderId = currOrder.Id,
                Amount = currOrder.TotalPrice,
                RedirectUrl = redirectUrl,
                CancelUrl = redirectUrl,
                OrderItems = orderItems.ToList()
            };

            var result = await _payOSService.createPaymentUrl(payOSRequestDTO);

            currOrder.PayOsUrl = result.checkoutUrl;
            currOrder.PaymentMethod = PaymentMethodEnum.PAYOS.ToString();
            currOrder.Status = OrderStatusEnum.PENDING.ToString();
            currOrder.UpdatedAt = DateTime.Now;

            await _orderRepository.UpdateOrderAsync(currOrder);

            return result.checkoutUrl;

        }






        public async Task<OrderWithItemsResponseDTO> CreateOrderWithItemsAsync(string token, OrderWithItemsRequestDTO orderWithItemsRequest)
        {

            var account = await _authenticationService.GetAccountByToken(token);


            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }


            if (orderWithItemsRequest == null || orderWithItemsRequest.Order == null || orderWithItemsRequest.OrderItems == null)
            {
                throw new ArgumentNullException(nameof(orderWithItemsRequest), "Order request or order items cannot be null or empty.");
            }

            var order = _mapper.Map<Order>(orderWithItemsRequest.Order);
            order.CustomerId = account.Id;
            order.Status = OrderStatusEnum.PENDING.ToString();
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;


            decimal totalPrice = 0;
            List<decimal> priceItems = new List<decimal>();

            // Tính Price
            foreach (var orderItemDto in orderWithItemsRequest.OrderItems)
            {
                var product = await _productService.GetProductById(orderItemDto.ProductId);

                // Kiểm tra tồn kho
                if (product.Stock < orderItemDto.Quantity)
                {
                    throw new ApiException(HttpStatusCode.NotFound, "Insufficient stock for this " + product.Name + " item");
                }
                decimal price = product.PriceSale ?? (product.Price - (product.Price * ((product.Discount ?? 0) / 100)));

                priceItems.Add(price);
                totalPrice += price * orderItemDto.Quantity;

                order.TotalPrice = totalPrice;
            }

            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            //======= /Tạo xong Order =============

            var orderItemResponses = new List<OrderItemResponseDTO>();

            // Tạo OrderItem
            int i = 0;
            foreach (var orderItemDto in orderWithItemsRequest.OrderItems)
            {

                var product = await _productService.GetProductById(orderItemDto.ProductId);


                var orderItemRequest = new OrderItemRequestDTO
                {
                    OrderId = createdOrder.Id, 
                    ProductId = orderItemDto.ProductId,
                    Quantity = orderItemDto.Quantity,
                    Price = priceItems[i]
                };

                var orderItem = _mapper.Map<OrderItem>(orderItemDto);
                orderItem.OrderId = createdOrder.Id;
                var createdOrderItemResponse = await _orderItemService.CreateOrderItemAsync(orderItemRequest);

                orderItemResponses.Add(createdOrderItemResponse);
                i++;
            }
           

            //=============/Create List OrderItem =========

            // Create Transaction
            var transactionRequest = new TransactionRequestDTO
            {
                OrderId = createdOrder.Id, 
                AccountId = createdOrder.CustomerId, 
                PaymentMethod = createdOrder.PaymentMethod.ToString(), 
                TotalPrice = createdOrder.TotalPrice, 
                Status = createdOrder.Status.ToString()
            };
            var createdTransaction= await _transactionService.CreateTransaction(transactionRequest);


            string? payOsUrl = null;

            if (createdOrder.PaymentMethod.Equals("PAYOS"))
            {

                PayOSRequestDTO payOSRequestDTO = new PayOSRequestDTO
                {
                    OrderId = createdOrder.Id,
                    Amount = createdOrder.TotalPrice,
                    RedirectUrl = "https://localhost:7160/api/payos?orderCode="+ createdOrder.Id,
                    CancelUrl = "https://localhost:7160/api/payos?orderCode=" + createdOrder.Id,
                    OrderItems = orderItemResponses.ToList()
                };

                var result = await _payOSService.createPaymentUrl(payOSRequestDTO);

                var currOrder = await _orderRepository.GetOrderByIdAsync(createdOrder.Id);
                currOrder.PayOsUrl = result.checkoutUrl;
                payOsUrl = currOrder.PayOsUrl;
                await _orderRepository.UpdateOrderAsync(currOrder);
            }

            var response = new OrderWithItemsResponseDTO
            {
                Order = _mapper.Map<OrderResponseDTO>(createdOrder),
                OrderItems = orderItemResponses,
                Transaction = createdTransaction

            };

            if (!string.IsNullOrEmpty(payOsUrl))
            {
                response.PayOsUrl = payOsUrl;
            }

            return response;
        }





    }
}
