using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Store
{
    public class StoreRequestDTO
    {
        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateOnly OpeningDay { get; set; }

        public TimeOnly OpeningHours { get; set; }

        public TimeOnly ClosingHours { get; set; }

        public string? BusinessLicense { get; set; }

        public int? AccountId { get; set; }
    }
}
