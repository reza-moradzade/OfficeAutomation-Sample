using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OfficeAutomation.Client.Interfaces;
using OfficeAutomation.Client.Models;

namespace OfficeAutomation.Client.Implementations
{
    // Implementation of ICartableClient using HttpClient
    public class CartableClient : ApiClientBase, ICartableClient
    {
        public CartableClient(HttpClient httpClient) : base(httpClient)
        {
        }

        // Call API to fetch cartable items
        public async Task<List<CartableItemDto>> GetAllAsync()
        {
            EnsureAuthorizationHeader(); // Ensure Bearer token is set

            var url = "api/Cartable";

            try
            {
                var result = await _http.GetFromJsonAsync<List<CartableItemDto>>(url, _jsonOptions);
                return result ?? new List<CartableItemDto>();
            }
            catch (HttpRequestException ex)
            {
                // Optionally log or rethrow for higher-level handling
                throw new Exception("Failed to fetch cartable items from API.", ex);
            }
        }
    }
}
