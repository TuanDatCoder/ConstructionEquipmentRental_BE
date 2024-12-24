using BuildLease.Data.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetCourses(string? token, int? page, int? size, string? sortBy);
    }
}
