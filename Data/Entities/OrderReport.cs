using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class OrderReport
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ReporterId { get; set; }

    public int? HandlerId { get; set; }

    public string? Reason { get; set; }

    public string? Details { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public virtual Account? Handler { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Account Reporter { get; set; } = null!;
}
