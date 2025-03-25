using AutoMapper;
using Data.DTOs.Product;
using Data.Enums;
using Data.Entities;
using Repositories.ProductRepos;
using Services.AuthenticationServices;
using Services.CloudinaryStorageServices; // Thay Firebase bằng Cloudinary
using Services.Helper.CustomExceptions;
using System.Net;
using System.IO;
using Data.DTOs.Category;
using Repositories.CategoryRepos;
using System.Collections.Generic;
using Repositories.StoreRepos;

namespace Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICloudinaryStorageService _cloudinaryStorageService; // Thay Firebase bằng Cloudinary
        private readonly IAuthenticationService _authenticationService;
        private readonly IStoreRepository _storeRepository;

        public ProductService(
            IMapper mapper,
            IProductRepository productRepository,
            ICloudinaryStorageService cloudinaryStorageService, // Thay Firebase bằng Cloudinary
            IAuthenticationService authenticationService,
            ICategoryRepository categoryRepository,
            IStoreRepository storeRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _cloudinaryStorageService = cloudinaryStorageService;
            _authenticationService = authenticationService;
            _categoryRepository = categoryRepository;
            _storeRepository = storeRepository;
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

        public async Task<ProductResponseDTO> CreateProductAsync(string token, ProductRequestDTO request, Stream fileStream, string fileName)
        {
            var account = await _authenticationService.GetAccountByToken(token);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }

            if (account.StoreId == null)
            {
                throw new Exception("This account does not have a store and cannot create products.");
            }
            var store = await _storeRepository.GetByIdAsync(account.StoreId.Value);
            if (store == null)
            {
                throw new Exception("Store not found. Please check again.");
            }

            if (account.Store.Status != StoreStatusEnum.ACTIVE.ToString())
            {
                throw new Exception("Your store has not been approved and you will not be able to create products.");
            }

            string fileUrl;
            if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
            {
                fileUrl = "https://res.cloudinary.com/your_cloud_name/image/upload/v1234567890/default_image.jpg"; // Thay bằng URL mặc định của Cloudinary
            }
            else
            {
                fileUrl = await _cloudinaryStorageService.UploadFileAsync(fileStream, fileName);
            }

            var product = _mapper.Map<Product>(request);
            product.Status = ProductStatusEnum.ACTIVE.ToString();
            product.DefaultImage = fileUrl;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            product.StoreId = account.StoreId.Value;

            await _productRepository.Add(product);
            return _mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<ProductResponseDTO> UpdateProduct(int id, string token, ProductUpdateRequestDTO request, Stream? fileStream, string? fileName)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var account = await CheckProductAndCurrentAccount(token, product.StoreId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            if (fileStream != null && !string.IsNullOrEmpty(fileName))
            {
                var fileUrl = await _cloudinaryStorageService.UploadFileAsync(fileStream, fileName);
                product.DefaultImage = fileUrl;
            }

            _mapper.Map(request, product);
            product.StoreId = account.StoreId.Value;
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

        public async Task<string> UpdatePictureAsync(int productId, string token, Stream fileStream, string fileName)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Product not found.");
            }
            CheckProductAndCurrentAccount(token, product.StoreId);

            var fileUrl = await _cloudinaryStorageService.UploadFileAsync(fileStream, fileName);
            product.DefaultImage = fileUrl;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.Update(product);

            return fileUrl;
        }

        public async Task<Account> CheckProductAndCurrentAccount(string token, int storeId)
        {
            var account = await _authenticationService.GetAccountByToken(token);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }

            if (account.StoreId == null)
            {
                throw new Exception("This account does not have a store and cannot create products.");
            }
            else if (storeId != account.StoreId)
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to update this product.");
            }
            return account;
        }

        public async Task<List<ProductResponseDTO>> GetProductsByProductIdAsync(int storeId)
        {
            var products = await _productRepository.GetProductsByStoreIdAsync(storeId);
            if (products == null || !products.Any())
            {
                throw new Exception($"No Products found for Store ID {storeId}.");
            }
            return _mapper.Map<List<ProductResponseDTO>>(products);
        }

        public async Task<CategoryWithProductsResponseDTO?> GetProductsByCategoryAsync(int categoryId)
        {
            var category = await _productRepository.GetProductsByCategoryAsync(categoryId);
            if (category == null)
                return null;
            return _mapper.Map<CategoryWithProductsResponseDTO>(category);
        }

        public async Task<List<CategoryWithProductsResponseDTO>> GetCategoriesAndTotalProductAsync(int? page, int? size)
        {
            try
            {
                var categories = await _categoryRepository.GetCategories(page, size);
                if (categories == null || !categories.Any())
                {
                    return new List<CategoryWithProductsResponseDTO>();
                }

                var categoriesAndProduct = new List<CategoryWithProductsResponseDTO>();
                foreach (var category in categories)
                {
                    var productsByCategory = await GetProductsByCategoryAsync(category.Id);
                    categoriesAndProduct.Add(productsByCategory);
                }

                return categoriesAndProduct;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}