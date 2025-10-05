using Birlik_Mobile.Helpers;
using Birlik_Mobile.Models.Request;
using Birlik_Mobile.Models.Response;
using Birlik_Mobile.Models.Result;
using System.Net.Http.Json;

namespace Birlik_Mobile.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        // 🔎 Variables expuestas a la UI para diagnóstico
        public string? LastRequestUrl { get; private set; }
        public string? LastServerSawApiKey { get; private set; }
        public string? LastError { get; private set; }

        public ApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ApiResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO request)
        {
            try
            {
                // ✅ Aseguramos que el header esté presente antes de enviar
                if (!_http.DefaultRequestHeaders.Contains("x-api-key"))
                {
                    _http.DefaultRequestHeaders.Add("x-api-key", ApiConstants.ApiKey);
                    Console.WriteLine("🔑 ApiKey agregada manualmente al HttpClient.");
                }

                // 🔍 DEPURACIÓN — Mostrar headers actuales y base address
                Console.WriteLine("🔍 DEPURACIÓN HTTPCLIENT:");
                foreach (var header in _http.DefaultRequestHeaders)
                    Console.WriteLine($" - {header.Key}: {string.Join(",", header.Value)}");

                Console.WriteLine($"🌐 BaseAddress: {_http.BaseAddress}");

                // 🌐 Construimos la URL final (para mostrar en UI)
                LastRequestUrl = new Uri(_http.BaseAddress!, "Auth/login").ToString();

                // 🚀 Ejecutamos la solicitud POST
                var response = await _http.PostAsJsonAsync("Auth/login", request);

                // 📨 Si el servidor devuelve un header diagnóstico, lo capturamos
                if (response.Headers.TryGetValues("X-Diag-ApiKey-Received", out var values))
                    LastServerSawApiKey = values.FirstOrDefault();

                // ⚠️ Manejo de errores de respuesta HTTP
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    LastError = error;
                    Console.WriteLine($"❌ Error HTTP: {response.StatusCode} - {error}");
                    return new ApiResult<LoginResponseDTO>
                    {
                        IsSuccess = false,
                        ErrorMessage = error
                    };
                }

                // ✅ Si todo va bien, deserializamos la respuesta
                var data = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                LastError = null;

                Console.WriteLine("✅ LoginAsync completado correctamente.");
                return new ApiResult<LoginResponseDTO>
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                // ⚠️ Captura de excepciones del cliente
                LastError = ex.Message;
                Console.WriteLine($"💥 Excepción en LoginAsync: {ex.Message}");
                return new ApiResult<LoginResponseDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
