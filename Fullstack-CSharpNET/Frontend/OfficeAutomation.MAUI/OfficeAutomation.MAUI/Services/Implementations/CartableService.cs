using OfficeAutomation.MAUI.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace OfficeAutomation.MAUI.Services
{
    /// <summary>
    /// Interface defining operations for retrieving cartable items.
    /// </summary>
    public interface ICartableService
    {
        Task<List<CartableItemDto>?> GetCartableItemsAsync();
    }

    /// <summary>
    /// Implementation of cartable service that fetches items from API.
    /// </summary>
    public class CartableService : ICartableService
    {
        private readonly HttpClient _http;
        private readonly AuthState _authState;

        public CartableService(HttpClient http, AuthState authState)
        {
            _http = http;
            _authState = authState;
        }

        /// <summary>
        /// Fetches cartable items from the API if the user is authenticated.
        /// </summary>
        public async Task<List<CartableItemDto>?> GetCartableItemsAsync()
        {
            // Check authentication state
            if (!_authState.IsAuthenticated || string.IsNullOrEmpty(_authState.Token))
                return null;

            // Set Authorization header for the request
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authState.Token);

            try
            {
                var response = await _http.GetAsync("api/Cartable");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode}");
                    return null;
                }

                // Deserialize JSON response
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
