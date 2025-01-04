using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.TransactionRepos
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public TransactionRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task Add(Transaction transaction)
        {
            try
            {
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when adding transaction: {ex.Message}");
            }
        }

        public async Task Delete(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions
                    .Include(p => p.Account)
                    .Include(p => p.Order)
                    .Include(p => p.WalletLog)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Transaction>> GetTransactions(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Transactions
                    .Include(p => p.Account)
                    .Include(p => p.Order)
                    .Include(p => p.WalletLog)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(Transaction transaction)
        {
            try
            {
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating transaction: {ex.Message}");
            }
        }
    }
}
