using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Store
{
    public class StoreResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Status { get; set; }

        public DateOnly? OpeningDay { get; set; }

        public TimeOnly? OpeningHours { get; set; }

        public TimeOnly? ClosingHours { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? BusinessLicense { get; set; }
    }
}
