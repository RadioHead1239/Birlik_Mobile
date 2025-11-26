using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birlik_Mobile.Models.Response
{
    public class PolizaResponseDTO
    {
        public int Id_poliza { get; set; }
        public string NumeroPoliza { get; set; }
        public string ProductoRamo { get; set; }
        public DateTime VigenciaInicio { get; set; }
        public DateTime VigenciaFin { get; set; }
        public bool EsActivo { get; set; }
        public string RutaDocumento { get; set; }
        public string NombreCompaniaSeguro { get; set; }
    }
}
