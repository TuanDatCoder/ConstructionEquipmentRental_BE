using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WalletRepos
{
    public interface IWalletRepository
    {
        Task<List<Wallet>> GetWallets(int? page, int? size);
        Task<Wallet> GetByIdAsync(int id);
        Task Add(Wallet wallet);
        Task Update(Wallet wallet);
        Task Delete(Wallet wallet);
    }
}
