﻿using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Product
{
    public class ProductUpdateRequestDTO
    {
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceSale { get; set; }
        public int Stock { get; set; }
        public ProductImageStatusEnum Status { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }
        public FuelTypeEnum FuelType { get; set; }
    }
}
