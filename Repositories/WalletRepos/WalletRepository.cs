using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using Repositories.TransactionRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WalletRepos
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public WalletRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task Add(Wallet wallet)
        {
            try
            {
                await _context.Wallets.AddAsync(wallet);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when adding wallet: {ex.Message}");
            }
        }

        public async Task Delete(Wallet wallet)
        {
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<Wallet> GetByIdAsync(int id)
        {
            return await _context.Wallets
                    .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Wallet>> GetWallets(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Wallets
                    .Include(p => p.Account)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(Wallet wallet)
        {
            try
            {
                _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating wallet: {ex.Message}");
            }
        }
    }
}
