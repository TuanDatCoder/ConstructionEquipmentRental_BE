using Data.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Category
{
    public class CategoryWithProductsResponseDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int TotalProducts { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductResponseDTO> Products { get; set; } = new List<ProductResponseDTO>();
    }
}
