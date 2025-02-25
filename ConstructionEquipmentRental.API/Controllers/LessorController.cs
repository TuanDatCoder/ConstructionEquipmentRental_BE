using Microsoft.AspNetCore.Mvc;
using Services.LessorServices;

namespace ConstructionEquipmentRental.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LessorController : ControllerBase
    {
        private readonly ILessorService _lessorService;

        public LessorController(ILessorService lessorService)
        {
            _lessorService = lessorService;
        }

        [HttpGet("lessor/{lessorId}")]
        public async Task<IActionResult> GetRevenueByLessor(int lessorId)
        {
            var revenue = await _lessorService.GetRevenueByLessorAsync(lessorId);
            return Ok(revenue);
        }
    }
}
