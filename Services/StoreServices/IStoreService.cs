using Data.DTOs.Product;
using Data.DTOs.Store;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StoreServices
{
    public interface IStoreService
    {
        Task<List<StoreResponseDTO>> GetStores(int? page, int? size);
        Task<StoreResponseDTO> GetStoreById(int id);
        Task<StoreResponseDTO> CreateStore(String token, StoreRequestDTO request);
        Task<StoreResponseDTO> UpdateStore(int id, StoreUpdateRequestDTO request);
        Task DeleteStore(int id);
        Task<StoreResponseDTO> ChangeStoreStatus(int id, StoreStatusEnum newStatus);
    }
}
