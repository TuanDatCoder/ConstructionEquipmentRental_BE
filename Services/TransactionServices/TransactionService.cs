using AutoMapper;
using Data.DTOs.Product;
using Data.DTOs.Transaction;
using Data.Entities;
using Data.Enums;
using Repositories.ProductRepos;
using Repositories.TransactionRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TransactionServices
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IMapper mapper, ITransactionRepository transactionRepository)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }
        public async Task<TransactionResponseDTO> CreateTransaction(TransactionRequestDTO request)
        {
            var transaction = _mapper.Map<Transaction>(request);
            transaction.CreatedAt = DateTime.UtcNow;
            await _transactionRepository.Add(transaction);
            return _mapper.Map<TransactionResponseDTO>(transaction);
        }

        public async Task DeleteTransaction(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                throw new Exception($"Transaction with ID {id} not found.");
            }

            await _transactionRepository.Delete(transaction);
        }

        public async Task<TransactionResponseDTO> GetTransactionById(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                throw new Exception($"Transaction with ID {id} not found.");
            }

            return _mapper.Map<TransactionResponseDTO>(transaction);
        }

        public async Task<List<TransactionResponseDTO>> GetTransactions(int? page, int? size)
        {
            var transactions = await _transactionRepository.GetTransactions(page, size);
            return _mapper.Map<List<TransactionResponseDTO>>(transactions);
        }

        public async Task<TransactionResponseDTO> UpdateTransaction(int id, TransactionRequestDTO request)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                throw new Exception($"Transaction with ID {id} not found.");
            }

            _mapper.Map(request, transaction);


            await _transactionRepository.Update(transaction);

            return _mapper.Map<TransactionResponseDTO>(transaction);
        }
    }
}
