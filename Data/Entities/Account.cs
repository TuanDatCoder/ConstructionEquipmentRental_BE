using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Account
{
    public int Id { get; set; }

    public int? StoreId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Picture { get; set; }

    public string? GoogleId { get; set; }

    public string? Role { get; set; }

    public string? Status { get; set; }

    public int? Points { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Order> OrderCustomers { get; set; } = new List<Order>();

    public virtual ICollection<OrderReport> OrderReportHandlers { get; set; } = new List<OrderReport>();

    public virtual ICollection<OrderReport> OrderReportReporters { get; set; } = new List<OrderReport>();

    public virtual ICollection<Order> OrderStaffs { get; set; } = new List<Order>();

    public virtual Store? Store { get; set; }
}
