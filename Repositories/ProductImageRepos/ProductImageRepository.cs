using Data.Entities;
using Data;
using Repositories.FeedbackRepos;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories.ProductImageRepos
{
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public ProductImageRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<ProductImage>> GetProductImages(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.ProductImages
                    .Include(p => p.Product)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductImage> GetByIdAsync(int id)
        {
            return await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task Add(ProductImage productImage)
        {
            try
            {
                await _context.ProductImages.AddAsync(productImage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when adding productImage: {ex.Message}");
            }
        }

        public async Task Update(ProductImage productImage)
        {
            try
            {
                _context.ProductImages.Update(productImage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating productImage: {ex.Message}");
            }
        }

        public async Task Delete(ProductImage productImage)
        {
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
        }



    }
}
