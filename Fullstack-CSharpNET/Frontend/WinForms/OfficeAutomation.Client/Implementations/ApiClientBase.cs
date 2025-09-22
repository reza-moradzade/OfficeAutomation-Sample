using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using OfficeAutomation.Client.Services;

namespace OfficeAutomation.Client.Implementations
{
    // Base class for API clients with HttpClient and JSON settings
    public abstract class ApiClientBase
    {
        protected readonly HttpClient _http;
        protected readonly JsonSerializerOptions _jsonOptions;

        protected ApiClientBase(HttpClient httpClient)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,   // ignore case in JSON
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // camelCase property names
            };
        }

        // Ensure Bearer token is set for authorized requests
        protected void EnsureAuthorizationHeader()
        {
            var token = SessionManager.Instance.AccessToken;
            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _http.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}
