using AutoMapper;
using BuildLease.Data.DTOs.Product;
using BuildLease.Data.Models.Enums;
using Data.Entities;
using Repositories.ProductRepos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepo _productRepo;

        public ProductService(IMapper mapper, IProductRepo productRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
        }

        public async Task<List<ProductResponseDTO>> GetProducts(int? page, int? size)
        {
            var products = await _productRepo.GetProducts(page, size);
            return _mapper.Map<List<ProductResponseDTO>>(products);
        }

        public async Task<ProductResponseDTO> GetProductById(int productId)
        {
            
            var product = await _productRepo.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            return _mapper.Map<ProductResponseDTO>(product);
        }


        public async Task<ProductResponseDTO> CreateProduct(ProductRequestDTO request)
        {
            var product = _mapper.Map<Product>(request);
            product.Status = ProductStatusEnum.AVAILABLE.ToString();
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepo.Add(product);
            return _mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<ProductResponseDTO> UpdateProduct(int productId, ProductUpdateRequestDTO request)
        {
            // Tìm sản phẩm theo ID
            var product = await _productRepo.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            _mapper.Map(request, product);

            product.UpdatedAt = DateTime.UtcNow;

            await _productRepo.Update(product);

            return _mapper.Map<ProductResponseDTO>(product);
        }
        public async Task DeleteProduct(int productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            await _productRepo.Delete(product); 
        }

    }
}
