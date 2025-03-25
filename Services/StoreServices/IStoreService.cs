using Data.DTOs.Order;
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
        Task<List<StoreResponseDTO>> GetStores();
        Task<StoreResponseDTO> GetStoreById(int id);
        Task<StoreResponseDTO> CreateStore(string token, StoreRequestDTO request, Stream fileStream, string fileName);
        Task<StoreResponseDTO> UpdateStore(int id, StoreUpdateRequestDTO request);
        Task DeleteStore(int id);
        Task<StoreResponseDTO> ChangeStoreStatus(string token, int id, StoreStatusEnum newStatus);
        Task<List<StoreResponseDTO>> GetStoresByStatus(StoreStatusEnum status);
        Task<StoreResponseDTO> GetStoresByLessor(string token);
    }
}
