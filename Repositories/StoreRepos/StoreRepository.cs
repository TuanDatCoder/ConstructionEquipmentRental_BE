﻿using Data.Entities;
using Data;
using Repositories.GenericRepos;
using Repositories.ProductRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories.StoreRepos
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public StoreRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<Store>> GetStores(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Stores
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

        public async Task<Store> GetByIdAsync(int id)
        {
            return await _context.Stores
                .Include(p => p.Account)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        //public async Task Add(Store store)
        //{
        //    try
        //    {
        //        await _context.Stores.AddAsync(store);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error when adding store: {ex.Message}");
        //    }
        //}
        public async Task Add(Store store)
        {
            try
            {
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                var detailedError = dbEx.InnerException?.Message ?? dbEx.Message;
                throw new Exception($"Database update error: {detailedError}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}");
            }
        }


        public async Task Update(Store store)
        {
            try
            {
                _context.Stores.Update(store);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when updating store: {ex.Message}");
            }
        }

        public async Task Delete(Store store)
        {
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }



    }
}

