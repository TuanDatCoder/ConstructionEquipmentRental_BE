using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.StoreRepos
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store> CreateStoreAsync(Store store);

        Task<Store> UpdateStoreAsync(Store store);

        Task<bool> DeleteStoreAsync(int storeId);

        Task<Store> GetStoreByIdAsync(int storeId);
    }
}
