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
                // ✅ Validación del parámetro
                if (idCliente <= 0)
                {
                    Console.WriteLine($"⚠️ PolizaService: IdCliente inválido ({idCliente})");
                    return new List<PolizaViewModel>();
                }

                // ✅ Obtener token
                var token = await _auth.GetTokenAsync();
                if (string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("⚠️ PolizaService: token vacío. No se hará la llamada protegida.");
                    return new List<PolizaViewModel>();
                }

                // ✅ Establecer header de autorización
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                // ✅ Log de la solicitud
                var url = $"Poliza/cliente/{idCliente}";
                Console.WriteLine($"🌐 PolizaService: GET {_http.BaseAddress}{url}");

                var response = await _http.GetAsync(url);

                // ✅ Log detallado del resultado
                Console.WriteLine($"📡 PolizaService: Status {(int)response.StatusCode} - {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ PolizaService: Error al obtener pólizas");
                    Console.WriteLine($"   Status: {response.StatusCode}");
                    Console.WriteLine($"   Contenido: {errorContent}");
                    return new List<PolizaViewModel>();
                }

                // ✅ Leer respuesta
                var dtoList = await response.Content.ReadFromJsonAsync<List<PolizaResponseDTO>>();

                if (dtoList == null || !dtoList.Any())
                {
                    Console.WriteLine($"⚠️ PolizaService: La API retornó lista vacía o null para IdCliente {idCliente}");
                    return new List<PolizaViewModel>();
                }

                Console.WriteLine($"✅ PolizaService: Se obtuvieron {dtoList.Count} pólizas para IdCliente {idCliente}");

                // ✅ Mapear a ViewModels
                var polizas = dtoList.Select(dto => new PolizaViewModel
                {
                    Id = dto.Id_poliza,
                    Numero = dto.NumeroPoliza ?? "S/N",
                    Ramo = dto.ProductoRamo ?? "Sin especificar",
                    Inicio = dto.VigenciaInicio,
                    Fin = dto.VigenciaFin,
                    EsActivo = dto.EsActivo,
                    Aseguradora = dto.NombreCompaniaSeguro ?? "Sin aseguradora",
                    Documento = dto.RutaDocumento,
                    DiasParaVencer = (int)(dto.VigenciaFin - DateTime.Now).TotalDays
                }).ToList();

                return polizas;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"🌐 Error de red en PolizaService: {httpEx.Message}");
                return new List<PolizaViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Excepción en PolizaService.ObtenerPolizasClienteAsync:");
                Console.WriteLine($"   Mensaje: {ex.Message}");
                Console.WriteLine($"   Stack: {ex.StackTrace}");
                return new List<PolizaViewModel>();
            }
        }
    }
}