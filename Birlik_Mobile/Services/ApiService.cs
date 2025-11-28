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

        public string? LastRequestUrl { get; private set; }
        public string? LastServerSawApiKey { get; private set; }
        public string? LastError { get; private set; }

        public ApiService(HttpClient http)
        {
            _http = http;

            // Configura la URL base según el entorno (DEBUG / RELEASE)
            _http.BaseAddress = new Uri(ApiConstants.BaseUrl);

            // Agrega el header de seguridad si aún no existe
            if (!_http.DefaultRequestHeaders.Contains("x-api-key"))
                _http.DefaultRequestHeaders.Add("x-api-key", ApiConstants.ApiKey);
        }

        public async Task<ApiResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO request)
        {
            try
            {
                Console.WriteLine($"🌐 BaseAddress: {_http.BaseAddress}");

                var response = await _http.PostAsJsonAsync("Auth/login", request);
                if (response.Headers.TryGetValues("X-Diag-ApiKey-Received", out var values))
                    LastServerSawApiKey = values.FirstOrDefault();

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    LastError = error;
                    return new ApiResult<LoginResponseDTO>
                    {
                        IsSuccess = false,
                        ErrorMessage = error
                    };
                }

                var data = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                LastError = null;

                return new ApiResult<LoginResponseDTO>
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                Console.WriteLine($"💥 Excepción en LoginAsync: {ex.Message}");
                return new ApiResult<LoginResponseDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ApiResult<PolizaResponseDTO>> GetPolizaClienteAsync(int idCliente)
        {
            try
            {
                var url = $"Poliza/cliente/{idCliente}";
                LastRequestUrl = url;


                var response = await _http.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    LastError = error;
                    return new ApiResult<PolizaResponseDTO>
                    {
                        IsSuccess = false,
                        ErrorMessage = error
                    };
                }

                var data = await response.Content.ReadFromJsonAsync<PolizaResponseDTO>();
                LastError = null;

                return new ApiResult<PolizaResponseDTO>
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                Console.WriteLine($"💥 Excepción en GetPolizaClienteAsync: {ex.Message}");
                return new ApiResult<PolizaResponseDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
