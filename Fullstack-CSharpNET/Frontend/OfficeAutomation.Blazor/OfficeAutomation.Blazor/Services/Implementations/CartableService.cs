using OfficeAutomation.Blazor.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace OfficeAutomation.Blazor.Services
{
    // Interface defining operations for retrieving cartable items
    public interface ICartableService
    {
        Task<List<CartableItemDto>?> GetCartableItemsAsync(); // Get list of cartable items
    }

    // Implementation of cartable service
    public class CartableService : ICartableService
    {
        private readonly HttpClient _http;
        private readonly AuthState _authState;

        public CartableService(HttpClient http, AuthState authState)
        {
            _http = http;
            _authState = authState;
        }

        // Fetches cartable items from API
        public async Task<List<CartableItemDto>?> GetCartableItemsAsync()
        {
            // Return null if user is not authenticated
            if (string.IsNullOrEmpty(_authState.Token))
                return null;

            // Set Authorization header with Bearer token
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authState.Token);

            try
            {
                var response = await _http.GetAsync("api/Cartable");

                if (!response.IsSuccessStatusCode)
                    return null;

                // Deserialize JSON response ignoring property case
                var data = await response.Content.ReadFromJsonAsync<List<CartableItemDto>>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cartable Exception: " + ex.Message);
                return null;
            }
        }
    }
}
