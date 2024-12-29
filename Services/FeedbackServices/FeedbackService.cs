using AutoMapper;
using Data.DTOs.Feedback;
using Data.DTOs.Product;
using Data.Entities;
using Data.Enums;
using Repositories.FeedbackRepos;
using Repositories.ProductRepos;
using Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FeedbackServices
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IMapper mapper, IFeedbackRepository feedbackRepository)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
        }

        public async Task<List<FeedbackResponseDTO>> GetFeedbacks(int? page, int? size)
        {
            var products = await _feedbackRepository.GetFeedbacks(page, size);
            return _mapper.Map<List<FeedbackResponseDTO>>(products);
        }

        public async Task<FeedbackResponseDTO> GetFeedbackById(int id)
        {

            var feedback = await _feedbackRepository.GetByIdAsync(id);

            if (feedback == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            return _mapper.Map<FeedbackResponseDTO>(feedback);
        }


        public async Task<FeedbackResponseDTO> CreateFeedback(FeedbackRequestDTO request)
        {
            var feeckback = _mapper.Map<Feedback>(request);
            //feeckback.Status = FeedbackStatusEnum.AVAILABLE.ToString();
            feeckback.CreatedAt = DateTime.UtcNow;
            feeckback.UpdatedAt = DateTime.UtcNow;
            await _feedbackRepository.Add(feeckback);
            return _mapper.Map<FeedbackResponseDTO>(feeckback);
        }

        public async Task<FeedbackResponseDTO> UpdateFeedback(int id, FeedbackRequestDTO request)
        {

            var feeckback = await _feedbackRepository.GetByIdAsync(id);

            if (feeckback == null)
            {
                throw new Exception($"Feedback with ID {id} not found.");
            }

            _mapper.Map(request, feeckback);

            feeckback.UpdatedAt = DateTime.UtcNow;

            await _feedbackRepository.Update(feeckback);

            return _mapper.Map<FeedbackResponseDTO>(feeckback);
        }
        public async Task DeleteFeedback(int id)
        {
            var feeckback = await _feedbackRepository.GetByIdAsync(id);

            if (feeckback == null)
            {
                throw new Exception($"Feedback with ID {id} not found.");
            }

            await _feedbackRepository.Delete(feeckback);
        }

        //public async Task<ProductResponseDTO> ChangeProductStatus(int productId, ProductStatusEnum newStatus)
        //{
        //    var existingProduct = await _feedbackRepository.GetByIdAsync(productId);
        //    if (existingProduct == null)
        //    {
        //        throw new KeyNotFoundException($"Product with ID {productId} not found.");
        //    }


        //    existingProduct.Status = newStatus.ToString();

        //    await _feedbackRepository.Update(existingProduct);

        //    return _mapper.Map<ProductResponseDTO>(existingProduct);
        //}

    }
}
