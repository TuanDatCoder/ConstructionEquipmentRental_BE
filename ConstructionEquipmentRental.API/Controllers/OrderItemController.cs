using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using Microsoft.AspNetCore.Mvc;
using Services.OrderItemServices;
using Services.OrderServices;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemResponseDTO>>> GetAllOrderItem()
        {
            var orderItem = await _orderItemService.GetAllOrderItemsAsync();
            return Ok(orderItem);
        }
        [HttpPost]
        public async Task<ActionResult<OrderItemResponseDTO>> CreateOrderItem([FromBody] OrderItemRequestDTO orderItemRequest)
        {
            if (orderItemRequest == null)
                return BadRequest("Order item request cannot be null");

            var createdOrderItem = await _orderItemService.CreateOrderItemAsync(orderItemRequest);
            return CreatedAtAction(nameof(GetAllOrderItem), new { id = createdOrderItem.Id }, createdOrderItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItemRequestDTO orderItemRequest)
        {
            try
            {
                var updatedOrderItem = await _orderItemService.UpdateOrderItemAsync(id, orderItemRequest);
                return Ok(updatedOrderItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            try
            {
                var result = await _orderItemService.DeleteOrderItemAsync(id);
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
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            try
            {
                var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);
                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
