using OfficeAutomation.MAUI.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace OfficeAutomation.MAUI.Services
{
    // Interface defining authentication operations
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(string username, string password); // Perform login
        Task LogoutAsync();                                               // Perform logout
        Task<bool> IsAuthenticatedAsync();                                // Check if user is authenticated
    }

    // Implementation of authentication service
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ITokenService _tokenService;
        private readonly AuthState _authState;

        public AuthService(HttpClient http, ITokenService tokenService, AuthState authState)
        {
            _http = http;
            _tokenService = tokenService;
            _authState = authState;
        }

        // Logs in the user and stores the token
        public async Task<AuthResponse?> LoginAsync(string username, string password)
        {
            try
            {
                // ✅ Prepare JSON payload
                var payload = new { userName = username, password = password };

                // ✅ Query parameters according to Swagger spec
                var query = "?captchaToken=test1234&clientType=maui&ipAddress=10.0.2.2&deviceInfo=android";

                var response = await _http.PostAsJsonAsync($"api/Auth/login{query}", payload);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Status: " + response.StatusCode);
                Console.WriteLine("Content: " + content);

                if (!response.IsSuccessStatusCode) return null;

                // ✅ Deserialize response ignoring property case
                var auth = await response.Content.ReadFromJsonAsync<AuthResponse>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (auth != null && !string.IsNullOrEmpty(auth.Token))
                {
                    // ✅ Save tokens securely (SecureStorage instead of localStorage)
                    await _tokenService.SaveTokenAsync(auth.Token, auth.RefreshToken);

                    // ✅ Set default Authorization header for future HTTP requests
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", auth.Token);

                    // ✅ Update shared authentication state
                    _authState.IsAuthenticated = true;
                    _authState.Token = auth.Token;
                    _authState.UserName = auth.UserName;
                    _authState.FullName = auth.FullName;
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
            await _tokenService.RemoveTokensAsync();          // Remove tokens from SecureStorage
            _http.DefaultRequestHeaders.Authorization = null; // Clear Authorization header
            _authState.Clear();                               // Reset authentication state
        }

        // Checks if a valid token exists
        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
