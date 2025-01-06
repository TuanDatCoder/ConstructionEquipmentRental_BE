using Data.DTOs.Feedback;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FeedbackServices
{
    public interface IFeedbackService
    {
        Task<List<FeedbackResponseDTO>> GetFeedbacks(int? page, int? size);
        Task<FeedbackResponseDTO> GetFeedbackById(int id);
        Task<FeedbackResponseDTO> CreateFeedback(FeedbackRequestDTO request);
        Task<FeedbackResponseDTO> UpdateFeedback(int id, FeedbackUpdateRequestDTO request);
        Task DeleteFeedback(int id);
        Task<FeedbackResponseDTO> ChangeFeedbackStatus(int id, FeedbackStatusEnum newStatus);
    }
}
