using Data.DTOs.Product;
using Data.DTOs;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.FeedbackServices;
using System.Net;
using Data.DTOs.Feedback;

namespace ConstructionEquipmentRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFeedbacks(int? page = 1, int? size = 10)
        {
            var result = await _feedbackService.GetFeedbacks(page, size);

            return Ok(new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View feedbacks successfully",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            try
            {
                var product = await _feedbackService.GetFeedbackById(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Feedback retrieved successfully",
                    Data = product
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

        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDTO
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid feedback data"
                });
            }

            var result = await _feedbackService.CreateFeedback(request);

            return StatusCode((int)HttpStatusCode.Created, new ApiResponseDTO
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Feedback created successfully",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] FeedbackUpdateRequestDTO request)
        {
            try
            {
                var updatedFeedback = await _feedbackService.UpdateFeedback(id, request);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Feedback updated successfully",
                    Data = updatedFeedback
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
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                await _feedbackService.DeleteFeedback(id);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Feedback deleted successfully"
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
        public async Task<IActionResult> ChangeFeedbackStatus(int id, [FromBody] FeedbackStatusEnum newStatus)
        {
            try
            {
                var updatedFeedback = await _feedbackService.ChangeFeedbackStatus(id, newStatus);

                return Ok(new ApiResponseDTO
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Feedback status updated successfully",
                    Data = updatedFeedback
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
