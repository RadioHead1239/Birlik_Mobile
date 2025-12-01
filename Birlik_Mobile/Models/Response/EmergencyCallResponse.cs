using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birlik_Mobile.Models.Response
{
    public class EmergencyCallResponse
    {
        public string Message { get; set; }

        public string ExecutiveSid { get; set; }
        public string ExecutiveStatus { get; set; }

        public string GerenciaSid { get; set; }
        public string GerenciaStatus { get; set; }
    }
}
