using Data.DTOs.OrderItem;
using Data.DTOs.OrderReport;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.OrderItemServices;
using Services.OrderReportServices;

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
            return Ok(orderReport);
        }
        [HttpPost]
        public async Task<ActionResult<OrderReportResponseDTO>> CreateOrderReport([FromBody] OrderReportRequestDTO orderReportRequest)
        {
            if (orderReportRequest == null)
                return BadRequest("Order Report request cannot be null");

            var createdOrderReport = await _orderReportService.CreateOrderReportAsync(orderReportRequest);
            return CreatedAtAction(nameof(GetAllOrderReport), new { id = createdOrderReport.Id }, createdOrderReport);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderReport(int id, [FromBody] OrderReportRequestDTO orderReportRequest)
        {
            try
            {
                var updatedOrderReport = await _orderReportService.UpdateOrderReportAsync(id, orderReportRequest);
                return Ok(updatedOrderReport);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderReport(int id)
        {
            try
            {
                var result = await _orderReportService.DeleteOrderReportAsync(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderReportById(int id)
        {
            try
            {
                var orderReport = await _orderReportService.GetOrderReportByIdAsync(id);
                return Ok(orderReport);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
