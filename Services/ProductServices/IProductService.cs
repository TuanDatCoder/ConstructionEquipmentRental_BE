using BuildLease.Data.DTOs.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetProducts(int? page, int? size);
    }
}
