using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birlik_Mobile.Helpers
{
    public static class ApiConstants
    {
#if DEBUG
        //  desarrollo
        public const string BaseUrl = "http://192.168.18.52:5277/api/";
#else
        //  producción
        public const string BaseUrl = "https://birlikapi.azurewebsites.net/api/";
#endif

        // 🔐 Clave de API (la misma que usas en el middleware)
        public const string ApiKey = "BirlikSuperKey_2025";
    }

}
