using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.BrandRepos
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public BrandRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetBrands(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Brands
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Brand> GetByIdAsync(int id)
        {
            return await _context.Brands
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Brand> Add(Brand brand)
        {
            try
            {
                await _context.Brands.AddAsync(brand);
                await _context.SaveChangesAsync();
                return brand;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when adding brand: {ex.Message}");
            }
        }

        public async Task<Brand> Update(Brand brand)
        {
            try
            {
                _context.Brands.Update(brand);
                await _context.SaveChangesAsync();
                return brand;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating brand: {ex.Message}");
            }
        }

        public async Task Delete(Brand brand)
        {
            try
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when deleting brand: {ex.Message}");
            }
        }
    }
}
