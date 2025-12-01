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

        public TwilioService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiConstants.BaseUrl)
            };

            _httpClient.DefaultRequestHeaders.Add("x-api-key", ApiConstants.ApiKey);
        }

        public async Task<ApiResult<EmergencyCallResponse>> MakeEmergencyFlowAsync(string to, string voiceUrl = "")
        {
            var dto = new EmergencyCallDto
            {
                To = to,
                VoiceUrl = voiceUrl
            };

            string json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Twilio/call-flow", content);

                string responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResult<EmergencyCallResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent
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
            catch (Exception ex)
            {
                return new ApiResult<EmergencyCallResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
