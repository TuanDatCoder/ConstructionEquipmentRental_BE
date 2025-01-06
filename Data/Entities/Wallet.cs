using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Wallet
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string? BankName { get; set; }

    public string? BankAccount { get; set; }

    public decimal Balance { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<WalletLog> WalletLogs { get; set; } = new List<WalletLog>();
}
