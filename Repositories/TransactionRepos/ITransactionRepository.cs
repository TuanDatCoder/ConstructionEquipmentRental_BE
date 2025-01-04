using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.TransactionRepos
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactions(int? page, int? size);
        Task<Transaction> GetByIdAsync(int id);
        Task Add(Transaction transaction);
        Task Update(Transaction transaction);
        Task Delete(Transaction transaction);
    }
}
