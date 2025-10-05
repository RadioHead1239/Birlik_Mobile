using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birlik_Mobile.Models.Request
{
    public class LoginRequestDTO
    {
        public string Usuario { get; set; }
        public string PasswordHash { get; set; }
    }
}
