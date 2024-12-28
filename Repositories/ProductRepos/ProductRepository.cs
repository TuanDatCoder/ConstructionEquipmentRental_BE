using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;


namespace Repositories.ProductRepos
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public ProductRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<Product>> GetProducts(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Include(p => p.Store)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task Add(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when adding product: {ex.Message}");
            }
        }

        public async Task Update(Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating product: {ex.Message}");
            }
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

    

    }
}
