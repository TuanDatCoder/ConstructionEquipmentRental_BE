using Data.Entities;
using Data;
using Repositories.FeedbackRepos;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CategoryRepos
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public CategoryRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
