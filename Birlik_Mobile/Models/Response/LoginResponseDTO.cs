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
        public UsuarioInfoDTO Usuario { get; set; } = new UsuarioInfoDTO();
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }


}
