using Data.DTOs.Wallet;
using Data.DTOs.WalletLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.WalletLogServices
{
    public interface IWalletLogService
    {
        Task<List<WalletLogResponseDTO>> GetWalletLogs(int? page, int? size);
        Task<WalletLogResponseDTO> GetWalletLogById(int id);
        Task<WalletLogResponseDTO> CreateWalletLog(WalletLogRequestDTO request);
        Task<WalletLogResponseDTO> UpdateWalletLog(int id, WalletLogRequestDTO request);
        Task DeleteWalletLog(int id);
    }
}
