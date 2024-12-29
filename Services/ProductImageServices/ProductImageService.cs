using AutoMapper;
using Data.DTOs.Product;
using Data.DTOs.ProductImage;
using Data.Entities;
using Repositories.ProductImageRepos;
using Repositories.ProductRepos;
using Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductImageServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IMapper _mapper;
        private readonly IProductImageRepository _productImageRepository;

        public ProductImageService(IMapper mapper, IProductImageRepository productImageRepository)
        {
            _mapper = mapper;
            _productImageRepository = productImageRepository;
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
            //product.Status = ProductImageStatusEnum.AVAILABLE.ToString();
           
            await _productImageRepository.Add(productImages);
            return _mapper.Map<ProductImageResponseDTO>(productImages);
        }

        public async Task<ProductImageResponseDTO> UpdateProductImage(int id, ProductImageRequestDTO request)
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

        //public async Task<ProductResponseDTO> ChangeProductStatus(int id, ProductImageStatusEnum newStatus)
        //{
        //    var existingProduct = await _productImageRepository.GetByIdAsync(id);
        //    if (existingProduct == null)
        //    {
        //        throw new KeyNotFoundException($"ProductImage with ID {id} not found.");
        //    }


        //    existingProduct.Status = newStatus.ToString();

        //    await _productImageRepository.Update(existingProduct);

        //    return _mapper.Map<ProductResponseDTO>(existingProduct);
        //}


    }
}
