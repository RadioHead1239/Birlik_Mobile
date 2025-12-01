using Birlik_Mobile.Helpers;
using Birlik_Mobile.Models.Request;
using Birlik_Mobile.Models.Response;
using Birlik_Mobile.Models.Result;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Birlik_Mobile.Services
{
    public class TwilioService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public TwilioService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;

            // ✅ NO agregues la API Key aquí si ya está en MauiProgram.cs
            // Solo configura la BaseAddress si no está configurada
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(ApiConstants.BaseUrl);
            }
        }

        public async Task<ApiResult<EmergencyCallResponse>> MakeEmergencyFlowAsync(int idCliente, string? voiceUrl = null)
        {
            try
            {
                // ✅ Obtener y agregar el token JWT
                var token = await _authService.GetTokenAsync();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                Console.WriteLine($"🚨 Iniciando llamada de emergencia para IdCliente: {idCliente}");
                Console.WriteLine($"🔑 Headers actuales:");
                foreach (var header in _httpClient.DefaultRequestHeaders)
                {
                    Console.WriteLine($"   {header.Key}: {string.Join(", ", header.Value)}");
                }

                var dto = new EmergencyCallDto
                {
                    IdCliente = idCliente,
                    VoiceUrl = voiceUrl
                };

                string json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Emergency/call-flow", content);
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"📡 Respuesta del servidor: {response.StatusCode}");
                Console.WriteLine($"📄 Contenido: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResult<EmergencyCallResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error {response.StatusCode}: {responseContent}"
                    };
                }

                var result = JsonSerializer.Deserialize<EmergencyCallResponse>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new ApiResult<EmergencyCallResponse>
                {
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"🌐 Error de red: {httpEx.Message}");
                return new ApiResult<EmergencyCallResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error de conexión: {httpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado: {ex.Message}");
                return new ApiResult<EmergencyCallResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = $"Error inesperado: {ex.Message}"
                };
            }
        }
    }
}