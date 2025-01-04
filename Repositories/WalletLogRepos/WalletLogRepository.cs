using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WalletLogRepos
{
    public class WalletLogRepository : GenericRepository<WalletLog>, IWalletLogRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public WalletLogRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task Add(WalletLog walletLog)
        {
            try
            {
                await _context.WalletLogs.AddAsync(walletLog);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when adding walletLog: {ex.Message}");
            }
        }

        public async Task Delete(WalletLog walletLog)
        {
            _context.WalletLogs.Remove(walletLog);
            await _context.SaveChangesAsync();
        }

        public async Task<WalletLog> GetByIdAsync(int id)
        {
            return await _context.WalletLogs
                .Include(p => p.Transaction)
                .Include(p => p.Wallet)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<WalletLog>> GetWalletLogs(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.WalletLogs
                    .Include(p => p.Transaction)
                    .Include(p => p.Wallet)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(WalletLog walletLog)
        {
            try
            {
                _context.WalletLogs.Update(walletLog);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating walletLog: {ex.Message}");
            }
        }
    }
}
