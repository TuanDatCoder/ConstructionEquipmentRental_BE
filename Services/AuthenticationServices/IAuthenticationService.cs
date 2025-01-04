using Data.DTOs.Account;
using Data.DTOs.Auth;
using Data.DTOs.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task ChangePassword(string token, ChangePasswordRequestDTO changePasswordRequestDTO);
        Task ForgotPassword(string email);
        Task<AccountInformationResponseDTO> GetUserInfor(string token);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task Logout(string refreshToken);
        Task RegisterCustomer(LoginRequestDTO loginRequestDTO);
        Task ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO);
        string GenerateOTP();
        Task<LoginResponseDTO> LoginGoogle(LoginGoogleRequestDTO loginGoogleRequestDTO);
        Task VerifyAccount(string token);
    }
}
