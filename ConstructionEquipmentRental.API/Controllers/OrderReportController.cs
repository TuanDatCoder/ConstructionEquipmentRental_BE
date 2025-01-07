using Data.DTOs;
using Data.DTOs.OrderItem;
using Data.DTOs.OrderReport;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.OrderItemServices;
using Services.OrderReportServices;
using System.Net;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderReportController : ControllerBase
    {
        private readonly IOrderReportService _orderReportService;

        public OrderReportController(IOrderReportService orderReportService)
        {
            _orderReportService = orderReportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReportResponseDTO>>> GetAllOrderReport()
        {
            var orderReport = await _orderReportService.GetAllOrderReportAsync();
            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View Order Report successfully",
                Data = orderReport
            });
        }
        [HttpPost]
        public async Task<ActionResult<OrderReportResponseDTO>> CreateOrderReport([FromBody] OrderReportRequestDTO orderReportRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid Order Report data"
                });
            }

            var createdOrderReport = await _orderReportService.CreateOrderReportAsync(orderReportRequest);
            return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Wallet created successfully",
                Data = createdOrderReport
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderReport(int id, [FromBody] OrderReportRequestDTO orderReportRequest)
        {
            try
            {
                var updatedOrderReport = await _orderReportService.UpdateOrderReportAsync(id, orderReportRequest);
                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Order Report updated successfully",
                    Data = updatedOrderReport
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
        public async Task<IActionResult> DeleteOrderReport(int id)
        {
            try
            {
                var result = await _orderReportService.DeleteOrderReportAsync(id);
                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Order Report deleted successfully"
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
        public async Task<IActionResult> GetOrderReportById(int id)
        {
            try
            {
                var orderReport = await _orderReportService.GetOrderReportByIdAsync(id);
                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Order Report retrieved successfully",
                    Data = orderReport
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

    }
}
