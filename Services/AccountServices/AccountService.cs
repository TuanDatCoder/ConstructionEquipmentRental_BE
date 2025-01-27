using AutoMapper;
using Data.DTOs.Account;
using Data.Entities;
using Data.Enums;
using Repositories.AccountRepos;
using Repositories.ProductRepos;
using Services.AuthenticationServices;
using Services.EmailServices;
using Services.FirebaseStorageServices;
using Services.Helper.CustomExceptions;
using Services.Helper.DecodeTokenHandler;
using Services.ProductServices;
using Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IDecodeTokenHandler _decodeTokenHandler; 
        private readonly IEmailService _emailService;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IAuthenticationService _authenticationService;

        public AccountService(IMapper mapper, IAccountRepository accountRepository, IDecodeTokenHandler decodeTokenHandler, IEmailService emailService, IFirebaseStorageService firebaseStorageService, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _decodeTokenHandler = decodeTokenHandler;
            _emailService = emailService;
            _firebaseStorageService = firebaseStorageService;
            _authenticationService = authenticationService;
        }



        public async Task<string> UploadProfilePictureAsync(int accountId, Stream fileStream, string fileName)
        {
            var account = await _accountRepository.GetAccountById(accountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }


            var uniqueFileName = await _firebaseStorageService.UploadFileAsync(fileStream, fileName);
            var fileUrl = _firebaseStorageService.GetSignedUrl(uniqueFileName);

            account.Picture = fileUrl;
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.Update(account);

            return fileUrl;
        }

         public async Task<string> UploadPictureAsync(string token, Stream fileStream, string fileName)
        {
            var account = await _authenticationService.GetAccountByToken(token);

            var uniqueFileName = await _firebaseStorageService.UploadFileAsync(fileStream, fileName);
            var fileUrl = _firebaseStorageService.GetSignedUrl(uniqueFileName);

            account.Picture = fileUrl;
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.Update(account);

            return fileUrl;
        }



        public async Task<bool> checkUsernameExisted(string username)
        {
            return (await _accountRepository.IsExistedByUsername(username));
        }

        public async Task<bool> checkEmailExisted(string email)
        {
            return (await _accountRepository.IsExistedByEmail(email));
        }

       


    }
}
