using System.Net.Http.Json;
using OfficeAutomationSuite.Shared.Models;

namespace OfficeAutomationSuite.Client.Services
{
    // Service for handling user authentication and registration
    public class UserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        // ---------------- Register a new user ----------------
        public async Task<string> RegisterAsync(UserRegisterDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/users/register", dto);
            if (response.IsSuccessStatusCode)
                return "Success"; // Registration succeeded
            else
                return await response.Content.ReadAsStringAsync(); // Return error message
        }

        // ---------------- Login user ----------------
        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/users/login", dto);
            if (!response.IsSuccessStatusCode)
                return null; // Invalid credentials

            // Read JWT token from server response
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return result?.Token;
        }
    }

    // Helper class for deserializing the JWT token response
    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
