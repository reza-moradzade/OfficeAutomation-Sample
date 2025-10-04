using System.Net.Http.Json;
using OfficeAutomationSuite.Shared.Models;

namespace OfficeAutomationSuite.Client.Services
{
    // Service for fetching cartable data from the server
    public class CartableService
    {
        private readonly HttpClient _http;

        public CartableService(HttpClient http)
        {
            _http = http;
        }

        // ---------------- Get all cartables ----------------
        public async Task<List<CartableDto>> GetAllCartablesAsync()
        {
            try
            {
                // Call API to fetch all cartable records
                var cartables = await _http.GetFromJsonAsync<List<CartableDto>>("api/cartable");
                return cartables ?? new List<CartableDto>(); // Return empty list if null
            }
            catch (Exception ex)
            {
                // Log any errors during the HTTP call
                Console.WriteLine($"Error fetching cartables: {ex.Message}");
                return new List<CartableDto>();
            }
        }
    }
}
