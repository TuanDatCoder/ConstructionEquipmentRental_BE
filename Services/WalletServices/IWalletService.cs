using Data.DTOs.Transaction;
using Data.DTOs.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.WalletServices
{
    public interface IWalletService
    {
        Task<List<WalletResponseDTO>> GetWallets(int? page, int? size);
        Task<WalletResponseDTO> GetWalletById(int id);
        Task<WalletResponseDTO> CreateWallet(WalletRequestDTO request);
        Task<WalletResponseDTO> UpdateWallet(int id, WalletRequestDTO request);
        Task DeleteWallet(int id);
    }
}
