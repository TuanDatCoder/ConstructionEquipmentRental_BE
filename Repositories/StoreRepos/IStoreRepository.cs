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
        Task<List<Store>> GetStores(int? page, int? size);
        Task<Store> GetByIdAsync(int id);
        Task Add(Store store);
        Task Update(Store store);
        Task Delete(Store store);
    }
}
