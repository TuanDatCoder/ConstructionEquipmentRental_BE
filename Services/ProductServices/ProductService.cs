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
        private readonly IProductRepository _productRepository;

        public ProductService(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<List<ProductResponseDTO>> GetProducts(int? page, int? size)
        {
            var products = await _productRepository.GetProducts(page, size);
            return _mapper.Map<List<ProductResponseDTO>>(products);
        }

        public async Task<ProductResponseDTO> GetProductById(int productId)
        {
            
            var product = await _productRepository.GetByIdAsync(productId);

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
            await _productRepository.Add(product);
            return _mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<ProductResponseDTO> UpdateProduct(int productId, ProductUpdateRequestDTO request)
        {
           
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            _mapper.Map(request, product);

            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.Update(product);

            return _mapper.Map<ProductResponseDTO>(product);
        }
        public async Task DeleteProduct(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            await _productRepository.Delete(product); 
        }

        public async Task<ProductResponseDTO> ChangeProductStatus(int productId, ProductStatusEnum newStatus)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            
            existingProduct.Status = newStatus.ToString();

            await _productRepository.Update(existingProduct);

            return _mapper.Map<ProductResponseDTO>(existingProduct);
        }


    }
}
