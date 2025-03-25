using AutoMapper;
using Data.DTOs.ProductImage;
using Data.Entities;
using Data.Enums;
using Repositories.ProductImageRepos;
using Repositories.ProductRepos;
using Services.AuthenticationServices;
using Services.CloudinaryStorageServices; // Thêm Cloudinary service
using Services.FirebaseStorageServices; // Giữ lại để comment
using Services.Helper.CustomExceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Services.ProductImageServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IMapper _mapper;
        private readonly IProductImageRepository _productImageRepository;
        // private readonly IFirebaseStorageService _firebaseStorageService; // Comment Firebase
        private readonly ICloudinaryStorageService _cloudinaryStorageService; // Thêm Cloudinary
        private readonly IAuthenticationService _authenticationService;
        private readonly IProductRepository _productRepository;

        public ProductImageService(
            IMapper mapper,
            IProductImageRepository productImageRepository,
            // IFirebaseStorageService firebaseStorageService, // Comment Firebase
            ICloudinaryStorageService cloudinaryStorageService, // Thêm Cloudinary
            IAuthenticationService authenticationService,
            IProductRepository productRepository)
        {
            _mapper = mapper;
            _productImageRepository = productImageRepository;
            // _firebaseStorageService = firebaseStorageService; // Comment Firebase
            _cloudinaryStorageService = cloudinaryStorageService; // Thêm Cloudinary
            _authenticationService = authenticationService;
            _productRepository = productRepository;
        }

        public async Task<List<ProductImageResponseDTO>> GetProductImages(int? page, int? size)
        {
            var productImages = await _productImageRepository.GetProductImages(page, size);
            return _mapper.Map<List<ProductImageResponseDTO>>(productImages);
        }

        public async Task<ProductImageResponseDTO> GetProductImageById(int id)
        {
            var productImages = await _productImageRepository.GetByIdAsync(id);
            if (productImages == null)
            {
                throw new Exception($"ProductImage with ID {id} not found.");
            }
            return _mapper.Map<ProductImageResponseDTO>(productImages);
        }

        public async Task<ProductImageResponseDTO> CreateProductImage(ProductImageRequestDTO request)
        {
            var productImages = _mapper.Map<ProductImage>(request);
            productImages.Status = ProductImageStatusEnum.ACTIVE.ToString();
            await _productImageRepository.Add(productImages);
            return _mapper.Map<ProductImageResponseDTO>(productImages);
        }

        public async Task<ProductImageResponseDTO> UpdateProductImage(int id, ProductImageUpdateRequestDTO request)
        {
            var productImages = await _productImageRepository.GetByIdAsync(id);
            if (productImages == null)
            {
                throw new Exception($"ProductImage with ID {id} not found.");
            }
            _mapper.Map(request, productImages);
            await _productImageRepository.Update(productImages);
            return _mapper.Map<ProductImageResponseDTO>(productImages);
        }

        public async Task DeleteProductImage(int id)
        {
            var product = await _productImageRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception($"ProductImage with ID {id} not found.");
            }
            await _productImageRepository.Delete(product);
        }

        public async Task<ProductImageResponseDTO> ChangeProductImageStatus(int id, ProductImageStatusEnum newStatus)
        {
            var existingProductImage = await _productImageRepository.GetByIdAsync(id);
            if (existingProductImage == null)
            {
                throw new KeyNotFoundException($"ProductImage with ID {id} not found.");
            }
            existingProductImage.Status = newStatus.ToString();
            await _productImageRepository.Update(existingProductImage);
            return _mapper.Map<ProductImageResponseDTO>(existingProductImage);
        }

        public async Task<List<ProductImageResponseDTO>> UploadMultipleProductImagesAsync(string token, List<Stream> files, List<string> names, int productId)
        {
            CheckProductAndCurrentAccount(token, productId);

            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("No files were uploaded.", nameof(files));
            }

            if (names == null || names.Count == 0 || files.Count != names.Count)
            {
                throw new ArgumentException("The number of files and names must be the same.", nameof(names));
            }

            var productImages = new List<ProductImageResponseDTO>();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var fileName = names[i];

                try
                {
                    var productImage = await CreateProductImageWithPictureAsync(productId, file, fileName);
                    productImages.Add(productImage);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error uploading file '{fileName}': {ex.Message}", ex);
                }
            }

            return productImages;
        }

        public async Task<ProductImageResponseDTO> CreateProductImageWithPictureAsync(int productId, Stream fileStream, string fileName)
        {
            // Comment lại logic Firebase
            /*
            if (_firebaseStorageService == null)
            {
                throw new InvalidOperationException("FirebaseStorageService is not initialized.");
            }
            */

            if (_cloudinaryStorageService == null)
            {
                throw new InvalidOperationException("CloudinaryStorageService is not initialized.");
            }

            string fileUrl;

            if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
            {
                // Giữ nguyên URL mặc định cũ của Firebase làm placeholder
                fileUrl = "https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/milling-machine.png?alt=media&token=98e4bca0-febe-471a-b807-d656ac81cb33";
            }
            else
            {
                try
                {
                    // Comment lại logic Firebase
                    /*
                    var uniqueFileName = await _firebaseStorageService.UploadFileAsync(fileStream, fileName);
                    fileUrl = _firebaseStorageService.GetSignedUrl(uniqueFileName);
                    */

                    // Logic mới với Cloudinary
                    fileUrl = await _cloudinaryStorageService.UploadFileAsync(fileStream, fileName);
                }
                catch (Exception ex)
                {
                    // throw new InvalidOperationException("Error occurred while uploading file to Firebase.", ex); // Comment Firebase
                    throw new InvalidOperationException("Error occurred while uploading file to Cloudinary.", ex);
                }
            }

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = fileUrl,
                Status = ProductImageStatusEnum.ACTIVE.ToString(),
            };

            await _productImageRepository.Add(productImage);
            return _mapper.Map<ProductImageResponseDTO>(productImage);
        }

        public async Task<List<ProductImageResponseDTO>> GetProductImagesByProductId(int productId)
        {
            var productImages = await _productImageRepository.GetProductImagesByProductId(productId);
            if (productImages == null || !productImages.Any())
            {
                throw new KeyNotFoundException($"No product images found for Product ID {productId}.");
            }
            return _mapper.Map<List<ProductImageResponseDTO>>(productImages);
        }

        public async Task<Account> CheckProductAndCurrentAccount(string token, int productId)
        {
            var account = await _authenticationService.GetAccountByToken(token);
            var product = await _productRepository.GetByIdAsync(productId);

            if (account.StoreId == null)
            {
                throw new Exception("This account does not have a store and cannot create products.");
            }
            else if (product.StoreId != account.StoreId)
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to update this product.");
            }
            return account;
        }
    }
}