using Data.DTOs.Product;
using Data.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetProducts(int? page, int? size);
        Task<ProductResponseDTO> GetProductById(int id);
        Task<ProductResponseDTO> CreateProductAsync(string token, ProductRequestDTO request, Stream fileStream, string fileName);
        Task<ProductResponseDTO> UpdateProduct(int id, string token, ProductUpdateRequestDTO request, Stream? fileStream, string? fileName);
        Task DeleteProduct(int id);
        Task<ProductResponseDTO> ChangeProductStatus(int id, ProductStatusEnum newStatus);
        Task<string> UpdatePictureAsync(int productId, string token, Stream fileStream, string fileName);

    }
}
