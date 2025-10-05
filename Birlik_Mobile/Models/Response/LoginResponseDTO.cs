using Birlik_Mobile.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birlik_Mobile.Models.Response
{
    public class LoginResponseDTO
    {
        public string Message { get; set; }
        public UsuarioInfoDTO Usuario { get; set; }


    }
}
