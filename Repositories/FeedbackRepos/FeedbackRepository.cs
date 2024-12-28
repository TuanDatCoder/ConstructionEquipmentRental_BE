using Data.Entities;
using Data;
using Repositories.GenericRepos;
using Microsoft.EntityFrameworkCore;

namespace Repositories.FeedbackRepos
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public FeedbackRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<Feedback>> GetFeedbacks(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Feedbacks
                    .Include(p => p.Account)
                    .Include(p => p.Product)
                    .Include(p => p.Order)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Feedback> GetByIdAsync(int id)
        {
            return await _context.Feedbacks
                .Include(p => p.Account)
                .Include(p => p.Product)
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task Add(Feedback feedback)
        {
            try
            {
                await _context.Feedbacks.AddAsync(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when adding product: {ex.Message}");
            }
        }

        public async Task Update(Feedback feedback)
        {
            try
            {
                _context.Feedbacks.Update(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating product: {ex.Message}");
            }
        }

        public async Task Delete(Feedback feedback)
        {
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
        }

    }
}
