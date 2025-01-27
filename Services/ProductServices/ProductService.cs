using AutoMapper;
using Data.DTOs.Product;
using Data.Enums;
using Data.Entities;
using Repositories.ProductRepos;
using Services.AuthenticationServices;
using Services.FirebaseStorageServices;
using Services.Helper.CustomExceptions;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;


namespace Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IAuthenticationService _authenticationService;


        public ProductService(IMapper mapper, IProductRepository productRepository, IFirebaseStorageService firebaseStorageService, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _firebaseStorageService = firebaseStorageService;
            _authenticationService = authenticationService;
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

            string fileUrl;
           
            if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
            {
               
                fileUrl = "https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/milling-machine.png?alt=media&token=98e4bca0-febe-471a-b807-d656ac81cb33";
            }
            else
            {
               
                var uniqueFileName = await _firebaseStorageService.UploadFileAsync(fileStream, fileName);
                fileUrl = _firebaseStorageService.GetSignedUrl(uniqueFileName);
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



        public async Task<ProductResponseDTO> UpdateProduct(int id, string token,ProductUpdateRequestDTO request, Stream? fileStream, string? fileName)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var account = await CheckProductAndCurrentAccount(token, product.StoreId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            if (fileStream != null && !string.IsNullOrEmpty(fileName))
            {
                var uniqueFileName = await _firebaseStorageService.UploadFileAsync(fileStream, fileName);
                var fileUrl = _firebaseStorageService.GetSignedUrl(uniqueFileName);

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

                        var uniqueFileName = await _firebaseStorageService.UploadFileAsync(fileStream, fileName);
            var fileUrl = _firebaseStorageService.GetSignedUrl(uniqueFileName);

            product.DefaultImage = fileUrl;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.Update(product);

            return fileUrl;
        }

        public async Task<Account> CheckProductAndCurrentAccount(string token, int storeId)
        {
            var account = await _authenticationService.GetAccountByToken(token);
            if (account.StoreId == null)
            {
                throw new Exception("This account does not have a store and cannot create products.");
            }else if (storeId != account.StoreId)
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to update this product.");
            }
            return account;
        }

    }
}
