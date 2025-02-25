using Data.Enums;
using Data.DTOs;
using Data.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using Services.OrderServices;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View Orders successfully",
                Data = result
            });
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder([FromBody] OrderRequestDTO orderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid Order data"
                });
            }

            var createdOrder = await _orderService.CreateOrderAsync(orderRequest);
            return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Order created successfully",
                Data = createdOrder
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderRequestDTO orderRequest)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(id, orderRequest);
                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Order updated successfully",
                    Data = updatedOrder
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Order deleted successfully"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Order retrieved successfully",
                    Data = order
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("payment")]
        public async Task<IActionResult> OrderPayment(OrderPaymentRequestDTO orderPaymentRequestDTO)
        {

            var paymentUrl = await _orderService.GetPaymentUrl(HttpContext, orderPaymentRequestDTO.orderId, orderPaymentRequestDTO.redirectUrl);

            var result = new OrderPaymentResponseDTO
            {
                orderId = orderPaymentRequestDTO.orderId,
                paymentUrl = paymentUrl
            };

            ApiResponseDTO response = new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get subscription paymentUrl successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }
        //


        [HttpPost("with-items")]
        [Authorize(Roles = "CUSTOMER, STAFF, LESSOR,  ADMIN")]
        public async Task<ActionResult<OrderWithItemsResponseDTO>> CreateOrderWithItemsAsync([FromBody] OrderWithItemsRequestDTO orderWithItemsRequest)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid Order with Items data"
                });
            }

            try
            {
                var createdOrder = await _orderService.CreateOrderWithItemsAsync(token, orderWithItemsRequest);
                return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.Created,
                    Message = "Order with items created successfully",
                    Data = createdOrder
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }


        [HttpPatch("{id}/status")]
        [Authorize(Roles = "STAFF, LESSOR,  ADMIN")]
        public async Task<IActionResult> ChangeOrderStatus(int id, [FromBody] OrderStatusEnum newStatus)
        {
            try
            {
                var updatedOrder = await _orderService.ChangeOrderStatus(id, newStatus);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Order status updated successfully",
                    Data = updatedOrder
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("by-customer")]
        [Authorize(Roles =  "CUSTOMER, STAFF, LESSOR,  ADMIN")]
        public async Task<IActionResult> GetOrdersByCustomerId()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            try
            {
                var orders = await _orderService.GetOrdersByCustomerAsync(token);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = $"Orders for Customer retrieved successfully",
                    Data = orders
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
        }



    }
}
