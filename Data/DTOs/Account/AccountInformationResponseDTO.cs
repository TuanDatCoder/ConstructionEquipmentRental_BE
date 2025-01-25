using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Account
{
    public class AccountInformationResponseDTO
    {
        public int? StoreId { get; set; }

        public string? Username { get; set; }
        public string? FullName { get; set; }
        public AccountGenderEnum Gender { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Picture { get; set; }

        public string? Role { get; set; }

        public string? StoreName { get; set; }

    }
}
