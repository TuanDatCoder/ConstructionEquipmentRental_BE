﻿using Data.Entities;
using Repositories.RefreshTokenRepos;
using Services.EmailServices;
using Services.Helper.CustomExceptions;
using Services.Helper.DecodeTokenHandler;
using Services.JWTServices;
using Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Services.Helper.VerifyCode;
using Data.DTOs.Password;
using Repositories.AccountRepos;
using Data.DTOs.Account;
using Data.DTOs.Product;
using AutoMapper;
using Data.Enums;
using Data.DTOs.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IEmailService _emailService;
        private readonly IJWTService _jWTService;
        private readonly VerificationCodeCache verificationCodeCache;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(
            IRefreshTokenRepository refreshTokenRepository,
            IDecodeTokenHandler decodeToken,
            IEmailService emailService,
            IJWTService jWTService,
            VerificationCodeCache verificationCodeCache,
            IAccountRepository accountRepository,
            IMapper mapper,
             IConfiguration config,
             IHttpContextAccessor httpContextAccessor
            )
        {
            this.verificationCodeCache = verificationCodeCache;

            _refreshTokenRepository = refreshTokenRepository;
            _decodeToken = decodeToken;
            _emailService = emailService;
            _jWTService = jWTService;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _config = config;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task ChangePassword(string token, ChangePasswordRequestDTO changePasswordRequestDTO)
        {
            var decode = _decodeToken.decode(token);

            var currentAccount = await _accountRepository.GetAccountByUsername(decode.username);


   
            if (currentAccount != null)
            {

                if (!PasswordHasher.VerifyPassword(changePasswordRequestDTO.OldPassword, currentAccount.Password))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "The old password incorrect");
                }

                if (changePasswordRequestDTO.OldPassword.Equals(changePasswordRequestDTO.NewPassword))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "The new password is the same with old password");
                }

                currentAccount.Password = PasswordHasher.HashPassword(changePasswordRequestDTO.NewPassword);

                await _accountRepository.Update(currentAccount);
            }
           
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }

        public async Task ForgotPassword(string email)
        {
            var currentAccount = await _accountRepository.GetAccountByEmail(email);

            if (currentAccount != null)
            {

                var otp = GenerateOTP();

                verificationCodeCache.Put(currentAccount.Username, otp, 5);

                await _emailService.SendAccountResetPassword(currentAccount.Username, currentAccount.Email, otp);

            }
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "Account does not exist");
            }
        }

        public async Task<AccountInformationResponseDTO> GetUserInfor(string token)
        {
            var decode = _decodeToken.decode(token);

            var currentAccount = await _accountRepository.GetAccountByUsername(decode.username);

            if (currentAccount != null)
            {

                return _mapper.Map<AccountInformationResponseDTO>(currentAccount);
            }

            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User not found");
            }
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var currentAccount = await _accountRepository.GetAccountByUsername(loginRequestDTO.Username);
           
          
            if (currentAccount != null)
            {
                if (currentAccount.Status.Equals(AccountStatusEnum.UNVERIFIED.ToString())) throw new ApiException(HttpStatusCode.BadRequest, "This account has been deactivated");

                if (PasswordHasher.VerifyPassword(loginRequestDTO.Password, currentAccount.Password))
                {
                    var token = _jWTService.GenerateJWT(currentAccount);

                    var refreshToken = _jWTService.GenerateRefreshToken();

                    var newRefreshToken = new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiredAt = DateTime.Now.AddDays(1),
                        AccountId = currentAccount.Id
                    };

                    await _refreshTokenRepository.Add(newRefreshToken);

                    var accLoginResponse = new LoginResponseDTO
                    {
                        Id = currentAccount.Id,
                        Username = currentAccount.Username,
                        Email = currentAccount.Email,
                        Role = currentAccount.Role,
                        AccessToken = token,
                        RefreshToken = refreshToken
                    };

                    return accLoginResponse;
                }
                else
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "Incorrect password");

                }
            }

            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }

        public async Task Logout(string refreshToken)
        {
            var currRefreshToken = await _refreshTokenRepository.GetByRefreshToken(refreshToken);

            if (currRefreshToken == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Refresh token does not exist");
            }

            await _refreshTokenRepository.Remove(currRefreshToken);
        }

        public Task RegisterCustomer(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            var currCustomer = await _accountRepository.GetAccountByEmail(resetPasswordRequestDTO.Email);

          

            if (currCustomer != null)
            {

                var otp = verificationCodeCache.Get(currCustomer.Username);

                if (otp == null || !otp.Equals(resetPasswordRequestDTO.OTP))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "OTP has expired or invalid OTP");
                }

                currCustomer.Password = PasswordHasher.HashPassword(resetPasswordRequestDTO.NewPassword);

                await _accountRepository.Update(currCustomer);

            }
           
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }

        public string GenerateOTP()
        {
            var otp = new Random().Next(100000, 999999).ToString();

            return otp;
        }

        public async Task<LoginResponseDTO> LoginGoogle(LoginGoogleRequestDTO loginGoogleRequestDTO)
        {
            string token = loginGoogleRequestDTO.Token;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ApiException(HttpStatusCode.Unauthorized, "Invalid Google token");
                    }

                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

                    if (jsonData == null)
                    {
                        throw new ApiException(HttpStatusCode.BadRequest, "Failed to parse Google user information.");
                    }

                    string email = jsonData.GetValueOrDefault("email")?.ToString() ?? string.Empty;
                    string givenName = jsonData.GetValueOrDefault("given_name")?.ToString() ?? string.Empty;
                    string picture = jsonData.GetValueOrDefault("picture")?.ToString() ?? string.Empty;

                    var currentAccount = await _accountRepository.GetAccountByEmail(email);
                    Account account;

                    if (currentAccount != null)
                    {
                        if (currentAccount.Status == AccountStatusEnum.UNVERIFIED.ToString())
                        {
                            throw new ApiException(HttpStatusCode.BadRequest, "This account has been deactivated.");
                        }

                        account = currentAccount;
                    }
                    else
                    {
                        account = new Account
                        {
                            Email = email,
                            Username = email.Split('@')[0],
                            Picture = picture,
                            Password = PasswordHasher.HashPassword(Guid.NewGuid().ToString()), // Random password
                            Status = AccountStatusEnum.UNVERIFIED.ToString(),
                            Role = AccountRoleEnum.CUSTOMER.ToString(),
                            Points = 0,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await _accountRepository.AddAccount(account);
                        await _emailService.SendRegistrationEmail(account.Username, account.Email);
                    }

                    // Authenticate the user and generate tokens
                    string newToken = _jWTService.GenerateJWT(account);
                    string refreshToken = _jWTService.GenerateRefreshToken();

                    var newRefreshToken = new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiredAt = DateTime.UtcNow.AddDays(1),
                        AccountId = account.Id
                    };

                    await _refreshTokenRepository.Add(newRefreshToken);

                    var loginResponseDTO = new LoginResponseDTO
                    {
                        Id = account.Id,
                        Username = account.Username,
                        Email = account.Email,
                        Role = account.Role,
                        AccessToken = newToken,
                        RefreshToken = refreshToken
                    };

                    return loginResponseDTO;
                }
            }
            catch (ApiException ex)
            {
                // Log error (optional)
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, "An unexpected error occurred: " + ex.Message);
            }
        }


        public async Task VerifyAccount(string token)
        {
            var decode = _decodeToken.decode(token);

            var currentAccount = await _accountRepository.GetAccountByUsername(decode.username);

            if (currentAccount == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Account does not exist");
            }

            if (currentAccount.Status != AccountStatusEnum.UNVERIFIED.ToString())
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Account is already verified");
            }

            currentAccount.Status = AccountStatusEnum.VERIFIED.ToString();

            await _accountRepository.Update(currentAccount);
        }


        public async Task AccountRegister(AccountRequestDTO accountRequestDTO)
        {
            if (await checkUsernameExisted(accountRequestDTO.Username))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Username has already been used by another user");
            }
            if (await checkEmailExisted(accountRequestDTO.Email))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Email has already been used by another user");
            }

            var account = _mapper.Map<Account>(accountRequestDTO);
            account.Password = PasswordHasher.HashPassword(accountRequestDTO.Password);
            account.Status = AccountStatusEnum.UNVERIFIED.ToString();
            account.CreatedAt = DateTime.Now;
            account.UpdatedAt = DateTime.Now;
            account.Points = 0;

            var accountId = await _accountRepository.AddAccount(account);

            var accountDone = await _accountRepository.GetAccountByUsername(account.Username);

            var token = _jWTService.GenerateJWT(accountDone);

            // Generate verification code
           // var verificationCode = Guid.NewGuid().ToString();
            verificationCodeCache.Put(account.Email, token, 30); // Expire in 30 minutes

            // Send email with verification link
            // Lấy host và port từ request hiện tại
            var request = _httpContextAccessor.HttpContext.Request;
            var host = $"{request.Scheme}://{request.Host}";
            var verificationLink = $"{host}/api/auth/verify?token={token}";
            //var verificationLink = $"https://localhost:7160/api/auth/verify?token={token}";
            await _emailService.SendRegistrationEmail(account.Username, account.Email, verificationLink);
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
