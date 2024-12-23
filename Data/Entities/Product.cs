using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public int StoreId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? DefaultImage { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public decimal? PriceSale { get; set; }

    public int? Stock { get; set; }

    public string? Status { get; set; }

    public DateOnly? DiscountStartDate { get; set; }

    public DateOnly? DiscountEndDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual Store Store { get; set; } = null!;
}
