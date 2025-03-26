using Data.DTOs.Account;
using Data.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AdminServices
{
    public interface IAdminService
    {
        Task<List<AccountResponseDTO>> GetAllAccountsAsync(int? page, int? size);
        Task<AdminDashboardDTO> GetDashboardStatsAsync();
    }
}
