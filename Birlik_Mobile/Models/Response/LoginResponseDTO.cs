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
        public int Id { get; set; }  
        public string Correo { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
        public int IdCliente { get; set; }
    }

}
