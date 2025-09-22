using OfficeAutomation.Blazor.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace OfficeAutomation.Blazor.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ITokenService _tokenService;

        public AuthService(HttpClient http, ITokenService tokenService)
        {
            _http = http;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse?> LoginAsync(string username, string password)
        {
            try
            {
                // Body JSON
                var payload = new { userName = username, password = password };
                // پارامترهای Query مطابق Swagger
                var query = "?captchaToken=test1234&clientType=web&ipAddress=172.0.0.1&deviceInfo=browser";

                var response = await _http.PostAsJsonAsync($"api/Auth/login{query}", payload);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Status: " + response.StatusCode);
                Console.WriteLine("Content: " + content);

                if (!response.IsSuccessStatusCode) return null;

                var auth = await response.Content.ReadFromJsonAsync<AuthResponse>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (auth != null && !string.IsNullOrEmpty(auth.Token))
                {
                    await _tokenService.SaveTokenAsync(auth.Token, auth.RefreshToken);
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", auth.Token);
                }

                return auth;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login Exception: " + ex.Message);
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            await _tokenService.RemoveTokensAsync();  // پاک کردن localStorage
            _http.DefaultRequestHeaders.Authorization = null; // پاک کردن Header
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
