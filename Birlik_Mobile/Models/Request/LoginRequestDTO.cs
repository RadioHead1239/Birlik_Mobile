using Birlik_Mobile.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birlik_Mobile.Models.Request
{
    public class LoginRequestDTO
    {
        public string Correo { get; set; }
        public string PasswordHash { get; set; }
    }

}
