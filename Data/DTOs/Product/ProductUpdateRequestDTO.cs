using BuildLease.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildLease.Data.DTOs.Product
{
    public class ProductUpdateRequestDTO
    {
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultImage { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceSale { get; set; }
        public int Stock { get; set; }
        public ProductStatusEnum Status { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
    }
}
