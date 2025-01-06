using AutoMapper;
using Data.DTOs.Account;
using Data.Entities;
using Data.Enums;
using Repositories.AccountRepos;
using Repositories.ProductRepos;
using Services.EmailServices;
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


        public AccountService(IMapper mapper, IAccountRepository accountRepository, IDecodeTokenHandler decodeTokenHandler, IEmailService emailService)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _decodeTokenHandler = decodeTokenHandler;
            _emailService = emailService;
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
