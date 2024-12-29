using Data.DTOs.Feedback;
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
        Task<FeedbackResponseDTO> UpdateFeedback(int id, FeedbackRequestDTO request);
        Task DeleteFeedback(int id);
    }
}
