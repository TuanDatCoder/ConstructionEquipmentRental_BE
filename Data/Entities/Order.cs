using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int StaffId { get; set; }

    public string? Status { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PurchaseMethod { get; set; }

    public string? RecipientName { get; set; }

    public string? RecipientPhone { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateOnly? DateOfReceipt { get; set; }

    public DateOnly? DateOfReturn { get; set; }

    public virtual Account Customer { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderReport> OrderReports { get; set; } = new List<OrderReport>();

    public virtual Account Staff { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
