using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FeedbackRepos
{
    public interface IFeedbackRepository
    {

        Task<List<Feedback>> GetFeedbacks(int? page, int? size);
        Task<Feedback> GetByIdAsync(int id);
        Task Add(Feedback feedback);
        Task Update(Feedback feedback);

        Task Delete(Feedback feedback);
    }
}
