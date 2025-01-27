using Data.DTOs.Product;
using Data.DTOs.ProductImage;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductImageServices
{
    public interface IProductImageService
    {
        Task<List<ProductImageResponseDTO>> GetProductImages(int? page, int? size);
        Task<ProductImageResponseDTO> GetProductImageById(int id);
        Task<ProductImageResponseDTO> CreateProductImage(ProductImageRequestDTO request);
        Task<ProductImageResponseDTO> UpdateProductImage(int id, ProductImageUpdateRequestDTO request);
        Task DeleteProductImage(int id);
        Task<ProductImageResponseDTO> ChangeProductImageStatus(int id, ProductImageStatusEnum newStatus);
        Task<List<ProductImageResponseDTO>> UploadMultipleProductImagesAsync(List<Stream> files, List<string> names, int productId);
    }
}
