using Data.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using Services.OrderServices;

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
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder([FromBody] OrderRequestDTO orderRequest)
        {
            if (orderRequest == null)
                return BadRequest("Order request cannot be null");

            var createdOrder = await _orderService.CreateOrderAsync(orderRequest);
            return CreatedAtAction(nameof(GetAllOrders), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderRequestDTO orderRequest)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(id, orderRequest);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrderAsync(id);
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
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
