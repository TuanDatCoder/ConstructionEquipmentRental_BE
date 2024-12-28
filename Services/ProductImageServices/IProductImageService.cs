using Data.DTOs.Product;
using Data.DTOs.ProductImage;
using Data.Models.Enums;
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
        Task<ProductImageResponseDTO> UpdateProductImage(int id, ProductImageRequestDTO request);
        Task DeleteProductImage(int id);
       // Task<ProductImageResponseDTO> ChangeProductImageStatus(int id, ProductImageStatusEnum newStatus);

    }
}
