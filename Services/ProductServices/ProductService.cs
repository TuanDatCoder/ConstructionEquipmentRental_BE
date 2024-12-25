using AutoMapper;
using BuildLease.Data.DTOs.Product;
using Data.Entities;
using Repositories.ProductRepos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepo _productRepo;

        public ProductService(IMapper mapper, IProductRepo productRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
        }

        public async Task<List<ProductResponseDTO>> GetProducts(int? page, int? size)
        {
            // Gọi repository để lấy dữ liệu sản phẩm có phân trang
            var products = await _productRepo.GetProducts(page, size);

            // Map từ Product entity sang ProductResponseDTO
            return _mapper.Map<List<ProductResponseDTO>>(products);
        }
    }
}
