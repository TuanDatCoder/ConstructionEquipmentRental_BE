using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
