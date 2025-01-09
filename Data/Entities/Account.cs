using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Account
{
    public int Id { get; set; }

    public int? StoreId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Picture { get; set; }

    public string? GoogleId { get; set; }

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? Points { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderReport> OrderReportHandlers { get; set; } = new List<OrderReport>();

    public virtual ICollection<OrderReport> OrderReportReporters { get; set; } = new List<OrderReport>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual Store? Store { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual Wallet? Wallet { get; set; }
}
