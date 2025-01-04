using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailServices
{
    public interface IEmailService 
    {
        Task SendAccountResetPassword(string fullName, string userEmail, string OTP);
        Task SendRegistrationEmail(string fullName, string userEmail);
        Task SendRegistrationEmail(string fullName, string userEmail, string verificationUrl);
    }
}
