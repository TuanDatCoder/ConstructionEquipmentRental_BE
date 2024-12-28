﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.OrderReport
{
    public class OrderReportRequestDTO
    {
        public int OrderId { get; set; }

        public int ReporterId { get; set; }

        public int? HandlerId { get; set; }

        public string? Reason { get; set; }

        public string? Details { get; set; }

        public DateTime? ResolvedAt { get; set; }

    }
}
