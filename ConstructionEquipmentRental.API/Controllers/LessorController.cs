using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Services.LessorServices;
using System.Net;

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



        [HttpGet("revenue")]
        [Authorize(Roles = "LESSOR")]
        public async Task<IActionResult> GetRevenueByLessor()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            try
            {
                var revenue = await _lessorService.GetRevenueByLessorAsync(token);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lessor revenue is successfully retrieved",
                    Data = revenue
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

        [HttpGet("orders")]
        [Authorize(Roles = "LESSOR")]
        public async Task<IActionResult> GetOrdersByLessor()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            try
            {
                var orders = await _lessorService.GetOrdersByLessorAsync(token);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Get successful order list",
                    Data = orders
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

        [HttpGet("summary")]
        [Authorize(Roles = "LESSOR")]
        public async Task<IActionResult> GetLessorSummary()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            try
            {
                var summary = await _lessorService.GetLessorSummaryAsync(token);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Lessor summary retrieved successfully",
                    Data = summary
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
