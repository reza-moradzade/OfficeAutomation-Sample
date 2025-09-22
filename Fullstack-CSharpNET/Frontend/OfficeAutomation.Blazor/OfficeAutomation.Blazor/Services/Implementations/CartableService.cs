using OfficeAutomation.Blazor.Models;
using OfficeAutomation.Blazor.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace OfficeAutomation.Blazor.Services
{
    public interface ICartableService
    {
        Task<List<CartableItemDto>?> GetCartableItemsAsync();
    }

    public class CartableService : ICartableService
    {
        private readonly HttpClient _http;
        private readonly AuthState _authState;

        public CartableService(HttpClient http, AuthState authState)
        {
            _http = http;
            _authState = authState;
        }

        public async Task<List<CartableItemDto>?> GetCartableItemsAsync()
        {
            if (string.IsNullOrEmpty(_authState.Token))
                return null;

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authState.Token);

            try
            {
                var response = await _http.GetAsync("api/Cartable");

                if (!response.IsSuccessStatusCode)
                    return null;

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
