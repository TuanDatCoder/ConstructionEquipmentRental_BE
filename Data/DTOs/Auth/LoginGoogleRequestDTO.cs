using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Auth
{
    public class LoginGoogleRequestDTO
    {
        public required string Token { get; set; }
    }
}
