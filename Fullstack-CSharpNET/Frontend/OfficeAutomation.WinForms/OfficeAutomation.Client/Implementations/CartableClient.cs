using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OfficeAutomation.Client.Interfaces;
using OfficeAutomation.Client.Models;

namespace OfficeAutomation.Client.Implementations
{
    public class CartableClient : ApiClientBase, ICartableClient
    {
        public CartableClient(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// فراخوانی API برای دریافت کارتابل
        /// </summary>
        public async Task<List<CartableItemDto>> GetAllAsync()
        {
            EnsureAuthorizationHeader();

            var url = "api/Cartable";

            try
            {
                var result = await _http.GetFromJsonAsync<List<CartableItemDto>>(url, _jsonOptions);
                return result ?? new List<CartableItemDto>();
            }
            catch (HttpRequestException ex)
            {
                // در صورت نیاز، می‌توان استثنا را مدیریت کرد یا دوباره پرتاب کرد
                throw new Exception("Failed to fetch cartable items from API.", ex);
            }
        }
    }
}
