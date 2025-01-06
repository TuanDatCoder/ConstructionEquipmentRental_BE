using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WalletLogRepos
{
    public interface IWalletLogRepository 
    {
        Task<List<WalletLog>> GetWalletLogs(int? page, int? size);
        Task<WalletLog> GetByIdAsync(int id);
        Task Add(WalletLog walletLog);
        Task Update(WalletLog walletLog);
        Task Delete(WalletLog walletLog);
    }
}
