using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.AdminServices;
using Services.BrandServices;
using System.Net;

namespace ConstructionEquipmentRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("account")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAccounts([FromQuery] int? page, [FromQuery] int? size)
        {

            //var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            string token = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Token is missing or invalid." });
            }

            token = token.Split(" ")[1];


            var result = await _adminService.GetAllAccountsAsync(token, page, size);
            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Accounts retrieved successfully",
                Data = result
            });
        }
    }
}
