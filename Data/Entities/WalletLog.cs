using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class WalletLog
{
    public int Id { get; set; }

    public int WalletId { get; set; }

    public int? TransactionId { get; set; }

    public string Type { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual Transaction? Transaction { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}
