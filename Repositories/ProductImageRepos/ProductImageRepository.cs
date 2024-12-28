using Data.Entities;
using Data;
using Repositories.FeedbackRepos;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ProductImageRepos
{
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public ProductImageRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }
    
    }
}
