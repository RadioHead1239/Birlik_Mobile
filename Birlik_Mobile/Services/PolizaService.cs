using Birlik_Mobile.Models.Response;
using Birlik_Mobile.Models.ViewModel;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Birlik_Mobile.Services
{
    public class PolizaService
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public PolizaService(HttpClient httpClient, AuthService auth)
        {
            _http = httpClient;
            _auth = auth;
        }

        public async Task<List<PolizaViewModel>> ObtenerPolizasClienteAsync(int idCliente)
        {
            try
            {
                var token = await _auth.GetTokenAsync();

                if (string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("⚠️ PolizaService: token vacío. No se hará la llamada protegida.");
                    return new List<PolizaViewModel>();
                }

                // Establecer header de autorización
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.GetAsync($"api/polizas/cliente/{idCliente}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ PolizaService: error al obtener pólizas: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return new List<PolizaViewModel>();
                }

                var dtoList = await response.Content.ReadFromJsonAsync<List<PolizaResponseDTO>>() ?? new List<PolizaResponseDTO>();

                return dtoList.Select(dto => new PolizaViewModel
                {
                    Id = dto.Id_poliza,
                    Numero = dto.NumeroPoliza,
                    Ramo = dto.ProductoRamo,
                    Inicio = dto.VigenciaInicio,
                    Fin = dto.VigenciaFin,
                    EsActivo = dto.EsActivo,
                    Aseguradora = dto.NombreCompaniaSeguro,
                    Documento = dto.RutaDocumento,
                    DiasParaVencer = (int)(dto.VigenciaFin - DateTime.Now).TotalDays
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Excepción en PolizaService.ObtenerPolizasClienteAsync: {ex}");
                return new List<PolizaViewModel>();
            }
        }
    }
}