using Data.Enums;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Product
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultImage { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceSale { get; set; }
        public int Stock { get; set; }
        public ProductImageStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string StoreName { get; set; }
        public decimal? Weight { get; set; }

        public string? Dimensions { get; set; }

        public FuelTypeEnum FuelType { get; set; }


    }
}
