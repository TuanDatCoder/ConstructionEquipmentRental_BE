using Data.DTOs.Product;
using Data.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetProducts(int? page, int? size);
        Task<ProductResponseDTO> GetProductById(int id);
        Task<ProductResponseDTO> CreateProduct(ProductRequestDTO request);
        Task<ProductResponseDTO> UpdateProduct(int id, ProductUpdateRequestDTO request);
        Task DeleteProduct(int id);
        Task<ProductResponseDTO> ChangeProductStatus(int id, ProductStatusEnum newStatus);

    }
}
