using OfficeAutomation.Blazor.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace OfficeAutomation.Blazor.Services
{
    // Interface defining authentication operations
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(string username, string password); // Perform login
        Task LogoutAsync();                                                // Perform logout
        Task<bool> IsAuthenticatedAsync();                                // Check if user is authenticated
    }

    // Implementation of authentication service
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ITokenService _tokenService;

        public AuthService(HttpClient http, ITokenService tokenService)
        {
            _http = http;
            _tokenService = tokenService;
        }

        // Logs in the user and stores the token
        public async Task<AuthResponse?> LoginAsync(string username, string password)
        {
            try
            {
                // Prepare JSON payload
                var payload = new { userName = username, password = password };

                // Query parameters according to Swagger spec
                var query = "?captchaToken=test1234&clientType=web&ipAddress=172.0.0.1&deviceInfo=browser";

                var response = await _http.PostAsJsonAsync($"api/Auth/login{query}", payload);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Status: " + response.StatusCode);
                Console.WriteLine("Content: " + content);

                if (!response.IsSuccessStatusCode) return null;

                // Deserialize response ignoring property case
                var auth = await response.Content.ReadFromJsonAsync<AuthResponse>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (auth != null && !string.IsNullOrEmpty(auth.Token))
                {
                    // Save tokens to localStorage
                    await _tokenService.SaveTokenAsync(auth.Token, auth.RefreshToken);

                    // Set default Authorization header for future HTTP requests
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

        // Logs out the user by clearing tokens and headers
        public async Task LogoutAsync()
        {
            await _tokenService.RemoveTokensAsync();         // Remove tokens from localStorage
            _http.DefaultRequestHeaders.Authorization = null; // Clear Authorization header
        }

        // Checks if a valid token exists
        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
