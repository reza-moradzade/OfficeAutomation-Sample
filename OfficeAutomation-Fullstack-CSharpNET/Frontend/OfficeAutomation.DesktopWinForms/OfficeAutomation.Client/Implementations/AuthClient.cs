using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using OfficeAutomation.Client.Interfaces;
using OfficeAutomation.Client.Models;
using OfficeAutomation.Client.Exceptions;

namespace OfficeAutomation.Client.Implementations
{
    // Implementation of IAuthClient using HttpClient
    public class AuthClient : ApiClientBase, IAuthClient
    {
        public AuthClient(HttpClient httpClient) : base(httpClient)
        {
        }

        // Authenticate a user with credentials and client/device info
        public async Task<AuthResultDto> LoginAsync(
            LoginDto loginDto,
            string captchaToken,
            string clientType,
            string ipAddress,
            string deviceInfo)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

            // Build query string parameters
            var qs = $"api/Auth/login?captchaToken={WebUtility.UrlEncode(captchaToken)}" +
                     $"&clientType={WebUtility.UrlEncode(clientType)}" +
                     $"&ipAddress={WebUtility.UrlEncode(ipAddress ?? string.Empty)}" +
                     $"&deviceInfo={WebUtility.UrlEncode(deviceInfo ?? string.Empty)}";

            var response = await _http.PostAsJsonAsync(qs, loginDto, _jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var txt = await response.Content.ReadAsStringAsync();
                throw new ApiException((int)response.StatusCode, txt);
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResultDto>(_jsonOptions)
                         ?? throw new ApiException((int)response.StatusCode, "Empty response body from auth.");

            return result;
        }
    }
}
