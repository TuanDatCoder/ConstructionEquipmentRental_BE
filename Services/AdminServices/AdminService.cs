using AutoMapper;
using Data.DTOs.Account;
using Data.DTOs.Brand;
using Data.Enums;
using Repositories.AccountRepos;
using Repositories.BrandRepos;
using Repositories.OrderRepos;
using Repositories.TransactionRepos;
using Services.BrandServices;
using Services.EmailServices;
using Services.Helper.CustomExceptions;
using Services.Helper.DecodeTokenHandler;
using Services.Helper.VerifyCode;
using Services.JWTServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.AdminServices
{
    public class AdminService: IAdminService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IDecodeTokenHandler _decodeToken;

        public AdminService(IAccountRepository accountRepository, IOrderRepository orderRepository, ITransactionRepository transactionRepository, IMapper mapper, IDecodeTokenHandler decodeToken)
        {
            _accountRepository = accountRepository;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _decodeToken = decodeToken;
        }

        //public async Task<List<AccountResponseDTO>> GetAllAccountsAsync(string token, int? page, int? size)
        //{

        //    var decodedToken = _decodeToken.decode(token);

        //    if (!decodedToken.roleName.Equals(AccountRoleEnum.ADMIN.ToString()))
        //    {
        //        throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
        //    }

        //    var brands = await _accountRepository.GetAccounts(page, size);
        //    return _mapper.Map<List<AccountResponseDTO>>(brands);
        //}

        public async Task<List<AccountResponseDTO>> GetAllAccountsAsync(int? page, int? size)
        {
            // Phân quyền đã được xử lý ở [Authorize] trong controller
            var accounts = await _accountRepository.GetAccounts(page, size);
            return _mapper.Map<List<AccountResponseDTO>>(accounts);
        }

    }
}
