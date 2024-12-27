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
    }
}