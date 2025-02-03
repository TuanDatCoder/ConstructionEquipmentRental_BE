using Data.DTOs.Product;
using Data.DTOs.Transaction;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TransactionServices
{
    public interface ITransactionService
    {
        Task<List<TransactionResponseDTO>> GetTransactions(int? page, int? size);
        Task<TransactionResponseDTO> GetTransactionById(int id);
        Task<TransactionResponseDTO> CreateTransaction(TransactionRequestDTO request);
        Task<TransactionResponseDTO> UpdateTransaction(int id, TransactionRequestDTO request);
        Task DeleteTransaction(int id);

        Task<TransactionResponseDTO> GetTransactionsByOrderId(int orderId);
        Task<List<TransactionResponseDTO>> GetTransactionsByAccountId(int accountId);
    

    }
}
