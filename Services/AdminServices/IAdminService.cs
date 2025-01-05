using Data.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AdminServices
{
    public interface IAdminService
    {
        Task<List<AccountResponseDTO>> GetAllAccountsAsync(string token, int? page, int? size);
    }
}
