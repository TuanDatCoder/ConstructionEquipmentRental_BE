using AutoMapper;
using Data.DTOs.Wallet;
using Data.DTOs.WalletLog;
using Data.Entities;
using Repositories.WalletLogRepos;
using Repositories.WalletRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.WalletLogServices
{
    public class WalletLogService : IWalletLogService
    {
        private readonly IMapper _mapper;
        private readonly IWalletLogRepository _walletLogRepository;

        public WalletLogService(IMapper mapper, IWalletLogRepository walletLogRepository)
        {
            _mapper = mapper;
            _walletLogRepository = walletLogRepository;
        }
        public async Task<WalletLogResponseDTO> CreateWalletLog(WalletLogRequestDTO request)
        {
            var walletLog = _mapper.Map<WalletLog>(request);
            walletLog.CreatedAt = DateTime.Now;
            await _walletLogRepository.Add(walletLog);
            return _mapper.Map<WalletLogResponseDTO>(walletLog);
        }

        public async Task DeleteWalletLog(int id)
        {
            var walletLog = await _walletLogRepository.GetByIdAsync(id);

            if (walletLog == null)
            {
                throw new Exception($"WalletLog with ID {id} not found.");
            }

            await _walletLogRepository.Delete(walletLog);
        }

        public async Task<WalletLogResponseDTO> GetWalletLogById(int id)
        {
            var walletLog = await _walletLogRepository.GetByIdAsync(id);

            if (walletLog == null)
            {
                throw new Exception($"WalletLog with ID {id} not found.");
            }

            return _mapper.Map<WalletLogResponseDTO>(walletLog);
        }

        public async Task<List<WalletLogResponseDTO>> GetWalletLogs(int? page, int? size)
        {
            var walletLogs = await _walletLogRepository.GetWalletLogs(page, size);
            return _mapper.Map<List<WalletLogResponseDTO>>(walletLogs);
        }

        public async Task<WalletLogResponseDTO> UpdateWalletLog(int id, WalletLogRequestDTO request)
        {
            var walletLog = await _walletLogRepository.GetByIdAsync(id);

            if (walletLog == null)
            {
                throw new Exception($"WalletLog with ID {id} not found.");
            }

            _mapper.Map(request, walletLog);


            await _walletLogRepository.Update(walletLog);

            return _mapper.Map<WalletLogResponseDTO>(walletLog);
        }
    }
}
