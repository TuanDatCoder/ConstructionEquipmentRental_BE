using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.StoreRepos
{
    public interface IStoreRepository
    {
        Task<List<Store>> GetStores();
        Task<Store> GetByIdAsync(int id);
        Task Add(Store store);
        Task Update(Store store);
        Task Delete(Store store);
        Task<List<Store>> GetStoresByStatus(StoreStatusEnum status);
        Task<Store> GetStoresByLessorIdAsync(int lessorId);
        Task<int> GetTotalStoresAsync();
    }
}
