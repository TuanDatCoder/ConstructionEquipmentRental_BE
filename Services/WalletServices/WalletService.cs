using AutoMapper;
using Data.DTOs.Transaction;
using Data.DTOs.Wallet;
using Data.Entities;
using Repositories.TransactionRepos;
using Repositories.WalletRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.WalletServices
{
    public class WalletService : IWalletService
    {
        private readonly IMapper _mapper;
        private readonly IWalletRepository _walletRepository;

        public WalletService(IMapper mapper, IWalletRepository walletRepository)
        {
            _mapper = mapper;
            _walletRepository = walletRepository;
        }
        public async Task<WalletResponseDTO> CreateWallet(WalletRequestDTO request)
        {
            var wallet = _mapper.Map<Wallet>(request);
            wallet.CreatedAt = DateTime.Now;
            await _walletRepository.Add(wallet);
            return _mapper.Map<WalletResponseDTO>(wallet);
        }

        public async Task DeleteWallet(int id)
        {
            var wallet = await _walletRepository.GetByIdAsync(id);

            if (wallet == null)
            {
                throw new Exception($"Wallet with ID {id} not found.");
            }

            await _walletRepository.Delete(wallet);
        }

        public async Task<WalletResponseDTO> GetWalletById(int id)
        {
            var wallet = await _walletRepository.GetByIdAsync(id);

            if (wallet == null)
            {
                throw new Exception($"Wallet with ID {id} not found.");
            }

            return _mapper.Map<WalletResponseDTO>(wallet);
        }

        public async Task<List<WalletResponseDTO>> GetWallets(int? page, int? size)
        {
            var wallets = await _walletRepository.GetWallets(page, size);
            return _mapper.Map<List<WalletResponseDTO>>(wallets);
        }

        public async Task<WalletResponseDTO> UpdateWallet(int id, WalletRequestDTO request)
        {
            var wallet = await _walletRepository.GetByIdAsync(id);

            if (wallet == null)
            {
                throw new Exception($"Wallet with ID {id} not found.");
            }

            _mapper.Map(request, wallet);


            await _walletRepository.Update(wallet);

            return _mapper.Map<WalletResponseDTO>(wallet);
        }
    }
}
