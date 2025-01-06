using AutoMapper;
using Data.DTOs.Product;
using Data.Enums;
using Data.Entities;
using Repositories.ProductRepos;


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

        public async Task<ProductResponseDTO> GetProductById(int id)
        {
            
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            return _mapper.Map<ProductResponseDTO>(product);
        }


        public async Task<ProductResponseDTO> CreateProduct(ProductRequestDTO request)
        {
            var product = _mapper.Map<Product>(request);
            product.Status = ProductImageStatusEnum.AVAILABLE.ToString();
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.Add(product);
            return _mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<ProductResponseDTO> UpdateProduct(int id, ProductUpdateRequestDTO request)
        {
           
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            _mapper.Map(request, product);

            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.Update(product);

            return _mapper.Map<ProductResponseDTO>(product);
        }
        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }

            await _productRepository.Delete(product); 
        }

        public async Task<ProductResponseDTO> ChangeProductStatus(int id, ProductStatusEnum newStatus)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            
            existingProduct.Status = newStatus.ToString();

            await _productRepository.Update(existingProduct);

            return _mapper.Map<ProductResponseDTO>(existingProduct);
        }


    }
}
