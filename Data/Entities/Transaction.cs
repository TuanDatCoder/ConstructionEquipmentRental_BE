using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Transaction
{
    public Guid Id { get; set; }

    public int OrderId { get; set; }

    public string? TransactionCode { get; set; }

    public decimal? Amount { get; set; }

    public string? Status { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
