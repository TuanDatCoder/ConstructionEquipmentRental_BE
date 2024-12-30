using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Product
{
    public class ProductRequestDTO
    {
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultImage { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public decimal? Weight { get; set; }

        public string? Dimensions { get; set; }

        public FuelTypeEnum FuelType { get; set; }

        
    }
}
