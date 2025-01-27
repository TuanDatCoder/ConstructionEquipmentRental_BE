using AutoMapper;
using Data.DTOs.Product;
using Data.DTOs.ProductImage;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Http;
using Repositories.ProductImageRepos;
using Repositories.ProductRepos;
using Services.AuthenticationServices;
using Services.FirebaseStorageServices;
using Services.ProductServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductImageServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IMapper _mapper;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IFirebaseStorageService _firebaseStorageService;
       // private readonly IAuthenticationService _authenticationService;
        public ProductImageService(IMapper mapper, IProductImageRepository productImageRepository, IFirebaseStorageService firebaseStorageService)
        {
            _mapper = mapper;
            _productImageRepository = productImageRepository;
            // _authenticationService = authenticationService;
            _firebaseStorageService = firebaseStorageService;
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
        public async Task<List<ProductImageResponseDTO>> UploadMultipleProductImagesAsync(List<Stream> files, List<string> names, int productId)
        {
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
                    // Log lỗi (nếu cần)
                    throw new InvalidOperationException($"Error uploading file '{fileName}': {ex.Message}", ex);
                }
            }

            return productImages;
        }



        public async Task<ProductImageResponseDTO> CreateProductImageWithPictureAsync(int productId, Stream fileStream, string fileName)
        {
            if (_firebaseStorageService == null)
            {
                throw new InvalidOperationException("FirebaseStorageService is not initialized.");
            }

            string fileUrl;

            if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
            {
                // Sử dụng URL mặc định
                fileUrl = "https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/milling-machine.png?alt=media&token=98e4bca0-febe-471a-b807-d656ac81cb33";
            }
            else
            {
                try
                {
                    var uniqueFileName = await _firebaseStorageService.UploadFileAsync(fileStream, fileName);
                    fileUrl = _firebaseStorageService.GetSignedUrl(uniqueFileName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error occurred while uploading file to Firebase.", ex);
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






    }
}
