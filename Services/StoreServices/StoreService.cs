﻿using AutoMapper;
using Data.DTOs.Product;
using Data.DTOs.Store;
using Data.Entities;
using Data.Enums;
using Repositories.AccountRepos;
using Repositories.ProductRepos;
using Repositories.StoreRepos;
using Services.AuthenticationServices;
using Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StoreServices
{
    public class StoreService : IStoreService
    {
        private readonly IMapper _mapper;
        private readonly IStoreRepository _storeRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthenticationService _authenticationService;

        public StoreService(IMapper mapper, IStoreRepository storeRepository, IAccountRepository accountRepository, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _storeRepository = storeRepository;
            _accountRepository = accountRepository;
            _authenticationService = authenticationService;
        }

        public async Task<List<StoreResponseDTO>> GetStores(int? page, int? size)
        {
            var products = await _storeRepository.GetStores(page, size);
            return _mapper.Map<List<StoreResponseDTO>>(products);
        }

        public async Task<StoreResponseDTO> GetStoreById(int id)
        {

            var product = await _storeRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception($"Store with ID {id} not found.");
            }

            return _mapper.Map<StoreResponseDTO>(product);
        }


        public async Task<StoreResponseDTO> CreateStore(String token, StoreRequestDTO request)
        {
            var currAccount = await _authenticationService.GetAccountByToken(token);

            if (currAccount == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }
            var store = _mapper.Map<Store>(request);
            store.AccountId = currAccount.Id;
            store.Status = StoreStatusEnum.PENDING.ToString();
            store.CreatedAt = DateTime.UtcNow;
            store.UpdatedAt = DateTime.UtcNow;
            await _storeRepository.Add(store);

            currAccount.StoreId = store.Id;
            await _accountRepository.Update(currAccount);
      

            return _mapper.Map<StoreResponseDTO>(store);
        }

        public async Task<StoreResponseDTO> UpdateStore(int id, StoreUpdateRequestDTO request)
        {

            var store = await _storeRepository.GetByIdAsync(id);

            if (store == null)
            {
                throw new Exception($"Store with ID {id} not found.");
            }

            _mapper.Map(request, store);

            store.UpdatedAt = DateTime.UtcNow;

            await _storeRepository.Update(store);

            return _mapper.Map<StoreResponseDTO>(store);
        }
        public async Task DeleteStore(int id)
        {
            var store = await _storeRepository.GetByIdAsync(id);

            if (store == null)
            {
                throw new Exception($"Store with ID {id} not found.");
            }

            await _storeRepository.Delete(store);
        }

        public async Task<StoreResponseDTO> ChangeStoreStatus(int id, StoreStatusEnum newStatus)
        {
            var existingStore = await _storeRepository.GetByIdAsync(id);
            if (existingStore == null)
            {
                throw new KeyNotFoundException($"Store with ID {id} not found.");
            }


            existingStore.Status = newStatus.ToString();

            await _storeRepository.Update(existingStore);

            return _mapper.Map<StoreResponseDTO>(existingStore);
        }

    }
}
