using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Store
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly OpeningDay { get; set; }

    public TimeOnly OpeningHours { get; set; }

    public TimeOnly ClosingHours { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? BusinessLicense { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
