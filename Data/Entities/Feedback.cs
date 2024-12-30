using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Feedback
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public bool HideName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
