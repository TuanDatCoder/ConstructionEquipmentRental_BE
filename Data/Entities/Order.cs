using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PurchaseMethod { get; set; } = null!;

    public string RecipientName { get; set; } = null!;

    public string RecipientPhone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateOnly DateOfReceipt { get; set; }

    public DateOnly DateOfReturn { get; set; }

    public string? PayOsUrl { get; set; }

    public virtual Account Customer { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderReport> OrderReports { get; set; } = new List<OrderReport>();

    public virtual Transaction? Transaction { get; set; }
}
