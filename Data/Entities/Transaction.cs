using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int AccountId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Order? Order { get; set; }

    public virtual WalletLog? WalletLog { get; set; }
}
