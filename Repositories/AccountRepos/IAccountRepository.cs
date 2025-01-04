﻿using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.AccountRepos
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByUsername(string username);
        Task<Account> GetAccountByEmail(string email);
        Task<bool> IsExistedByEmail(string email);
        Task<bool> IsExistedByUsername(string username);
        Task<int> AddAccount(Account account);
        Task<Account> Update(Account account);
        Task<Account> GetAccountById(int accountId);
    }
}
