using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birlik_Mobile.Models.ViewModel
{
    public class PolizaViewModel
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Ramo { get; set; }
        public string Estado { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public bool EsActivo { get; set; }
        public string Aseguradora { get; set; }
        public string Documento { get; set; }

        public int DiasParaVencer { get; set; }
        public string EstadoVisual => EsActivo ? ObtenerEstado() : "Vencida";

        private string ObtenerEstado()
        {
            if (!EsActivo) return "Vencida";

            var dias = (Fin - DateTime.Now).TotalDays;

            if (dias <= 0) return "Vencida";
            if (dias <= 30) return "ProximaVencer";

            return "Activa";
        }
    }

}
