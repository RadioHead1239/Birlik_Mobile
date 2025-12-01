using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birlik_Mobile.Models.Request
{
    public class EmergencyCallDto
    {
        public int IdCliente { get; set; }
        public string? VoiceUrl { get; set; }
    }
}