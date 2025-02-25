using AutoMapper;
using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using Data.DTOs.PayOS;
using Data.DTOs.Product;
using Data.DTOs.Transaction;
using Data.Entities;
using Data.Enums;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;
using Repositories.OrderRepos;
using Repositories.ProductRepos;
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
        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly IPayOSService _payOSService;
        private readonly IAuthenticationService _authenticationService;
        private readonly string _frontendUrl;

        public OrderService(IConfiguration config, IOrderRepository orderRepository, IOrderItemService orderItemService, IMapper mapper, IPayOSService payOSService, IProductService productService, ITransactionService transactionService, IAuthenticationService authenticationService, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderItemService = orderItemService;
            _productService = productService;
            _transactionService = transactionService;
            _mapper = mapper;
            _payOSService= payOSService;
            _authenticationService= authenticationService;
            _productRepository= productRepository;
            #pragma warning disable CS8601
            _frontendUrl = config["Environment:FE_URL"];
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

            if (order.DateOfReceipt >= order.DateOfReturn)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Receive date must be less than pay date.");
            }

            var rentalDays = (DateTime.Parse(order.DateOfReturn.ToString()) - DateTime.Parse(order.DateOfReceipt.ToString())).Days;

            // Kiểm tra nếu rentalDays nhỏ hơn 1 ngày thì throw exception
            if (rentalDays < 1)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Rental days must be greater than 0.");
            }

            decimal totalPrice = 0;
            List<decimal> priceItems = new List<decimal>();

            // Tính Price
            int checkId = -1;
            foreach (var orderItemDto in orderWithItemsRequest.OrderItems)
            {

                var product = await _productService.GetProductById(orderItemDto.ProductId);


                //kiểm tra xem có thuộc 1 cửa hàng đó không.
                if (checkId == -1) checkId = product.StoreId; // bắt thằng đầu tiên
                else
                {
                    if(checkId != product.StoreId)
                        throw new ApiException(HttpStatusCode.BadRequest, "Only order common products in one store");
                    checkId = product.StoreId;
                }
                

                // Kiểm tra tồn kho
                if (product.Stock < orderItemDto.Quantity)
                {
                    throw new ApiException(HttpStatusCode.NotFound, "Insufficient stock for this " + product.Name + " item");
                }

                decimal price = product.PriceSale ?? (product.Price - (product.Price * ((product.Discount ?? 0) / 100)));

                priceItems.Add(price);
                totalPrice += price * orderItemDto.Quantity;

               
            }
            // tính giá cho thuê chỉ theo ngày
            order.TotalPrice = totalPrice * rentalDays;

            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            //======= /Tạo xong Order =============

            var orderItemResponses = new List<OrderItemResponseDTO>();

            // Tạo OrderItem
            int i = 0;
            foreach (var orderItemDto in orderWithItemsRequest.OrderItems)
            {

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

                var productDTO = await _productRepository.GetByIdAsync(orderItemDto.ProductId);
                   
                    productDTO.Stock = productDTO.Stock - orderItemDto.Quantity;

                await _productRepository.Update(productDTO);

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

            // chuyển sang payos
            if (createdOrder.PaymentMethod.Equals("PAYOS"))
            {

                PayOSRequestDTO payOSRequestDTO = new PayOSRequestDTO
                {
                    OrderId = createdOrder.Id,
                    Amount = createdOrder.TotalPrice,
                    RedirectUrl = $"{_frontendUrl}/paysuccess?orderCode={createdOrder.Id + 10000}",
                    CancelUrl = $"{_frontendUrl}/paycancel?orderCode={createdOrder.Id + 10000}",
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


        public async Task<OrderResponseDTO> ChangeOrderStatus(int id, OrderStatusEnum newStatus)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            existingOrder.Status = newStatus.ToString();

            await _orderRepository.UpdateOrderAsync(existingOrder);

            return _mapper.Map<OrderResponseDTO>(existingOrder);
        }


        public async Task<List<OrderResponseDTO>> GetOrdersByCustomerAsync(string token)
        {

            var account = await _authenticationService.GetAccountByToken(token);


            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(account.Id);

            if (orders == null || !orders.Any())
            {
                throw new Exception($"No Orders found for Customer ID {account.Id}.");
            }

            return _mapper.Map<List<OrderResponseDTO>>(orders);
        }



    }
}
