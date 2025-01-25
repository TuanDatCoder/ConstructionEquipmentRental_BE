using Data.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AccountServices
{
    public interface IAccountService
    {
        Task<string> UploadProfilePictureAsync(int accountId, Stream fileStream, string fileName);
        Task<string> UploadPictureAsync(string token, Stream fileStream, string fileName);
    }
}
