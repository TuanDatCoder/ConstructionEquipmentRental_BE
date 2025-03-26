using AutoMapper;
using Data.DTOs.Order;
using Data.DTOs.Product;
using Data.DTOs.ProductImage;
using Data.DTOs.Store;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.AccountRepos;
using Repositories.ProductRepos;
using Repositories.StoreRepos;
using Services.AuthenticationServices;
using Services.EmailServices;
using Services.FirebaseStorageServices;
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
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IEmailService _emailService;

        public StoreService(IMapper mapper, IStoreRepository storeRepository, IAccountRepository accountRepository, IAuthenticationService authenticationService, IFirebaseStorageService firebaseStorageService, IEmailService emailService)
        {
            _mapper = mapper;
            _storeRepository = storeRepository;
            _accountRepository = accountRepository;
            _authenticationService = authenticationService;
            _firebaseStorageService = firebaseStorageService;
            _emailService = emailService;
        }

        public async Task<List<StoreResponseDTO>> GetStores()
        {
            var stores = await _storeRepository.GetStores();
            return _mapper.Map<List<StoreResponseDTO>>(stores);
        }

        public async Task<List<StoreResponseDTO>> GetStoresByStatus(StoreStatusEnum status)
        {
            var stores = await _storeRepository.GetStoresByStatus(status);
            return _mapper.Map<List<StoreResponseDTO>>(stores);
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


    



        public async Task<StoreResponseDTO> CreateStore(string token, StoreRequestDTO request, Stream fileStream, string fileName)
        {

            var currAccount = await _authenticationService.GetAccountByToken(token);


            if (currAccount == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }


            string fileUrl;

            if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
            {

                fileUrl = "https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/icons8-store-100.png?alt=media&token=ba0eef19-bad1-4e68-9784-c8ad6e482810";
            }
            else
            {

                var uniqueFileName = await _firebaseStorageService.UploadFileAsync(fileStream, fileName);
                fileUrl = _firebaseStorageService.GetSignedUrl(uniqueFileName);
            }


            var store = _mapper.Map<Store>(request);
            store.AccountId = currAccount.Id;
            store.Status = StoreStatusEnum.PENDING.ToString();
            store.BusinessLicense = fileUrl;
            store.CreatedAt = DateTime.UtcNow;
            store.UpdatedAt = DateTime.UtcNow;
            await _storeRepository.Add(store);

            currAccount.StoreId = store.Id;
            await _accountRepository.Update(currAccount);
            await _emailService.SendStoreCreationEmail(currAccount.FullName, currAccount.Email, store.Name);

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

        public async Task<StoreResponseDTO> ChangeStoreStatus(string token, int id, StoreStatusEnum newStatus)
        {
            var existingStore = await _storeRepository.GetByIdAsync(id);
            if (existingStore == null)
            {
                throw new KeyNotFoundException($"Store with ID {id} not found.");
            }

            var admin = await _authenticationService.GetAccountByToken(token);
            if (admin == null)
            {
                throw new UnauthorizedAccessException("Admin not found.");
            }

            existingStore.Status = newStatus.ToString();
            await _storeRepository.Update(existingStore);

            var owner = await _accountRepository.GetAccountById(existingStore.Account.Id);

            if (newStatus == StoreStatusEnum.ACTIVE)
            {
                await _emailService.SendApprovalEmail(owner.FullName, owner.Email, existingStore.Name, admin.FullName);
            }
            else if (newStatus == StoreStatusEnum.CANCELLED)
            {
                await _emailService.SendRejectionEmail(owner.FullName, owner.Email, existingStore.Name, admin.FullName, "Chúng tôi phát hiện rằng giấy phép kinh doanh của bạn gửi cho chúng tôi đang gặp vấn đề. Hiện tại chưa đạt yêu của chúng tôi");
            }

            return _mapper.Map<StoreResponseDTO>(existingStore);
        }
        public async Task<StoreResponseDTO> GetStoresByLessor(string token)
        {
            var account = await _authenticationService.GetAccountByToken(token);

            if (account == null)
            {
                throw new UnauthorizedAccessException("Invalid account or token.");
            }

            var store = await _storeRepository.GetStoresByLessorIdAsync(account.Id);

            if (store == null)
            {
                throw new Exception($"No Store found for Lessor ID {account.Id}.");
            }

            return _mapper.Map<StoreResponseDTO>(store);
        }
     

    }
}
