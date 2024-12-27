using BuildLease.Data.DTOs.Product;
using BuildLease.Data.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetProducts(int? page, int? size);
        Task<ProductResponseDTO> GetProductById(int productId);
        Task<ProductResponseDTO> CreateProduct(ProductRequestDTO productCreateDTO);
        Task<ProductResponseDTO> UpdateProduct(int productId, ProductUpdateRequestDTO request);
        Task DeleteProduct(int productId);
        Task<ProductResponseDTO> ChangeProductStatus(int productId, ProductStatusEnum newStatus);

    }
}
