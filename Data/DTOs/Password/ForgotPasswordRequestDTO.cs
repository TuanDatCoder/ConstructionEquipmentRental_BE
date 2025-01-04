using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Password
{
    public class ForgotPasswordRequestDTO
    {
        public required string email { get; set; }
    }
}
