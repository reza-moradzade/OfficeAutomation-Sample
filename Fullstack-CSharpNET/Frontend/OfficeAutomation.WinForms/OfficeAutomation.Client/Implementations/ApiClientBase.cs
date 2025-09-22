using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using OfficeAutomation.Client.Services;

namespace OfficeAutomation.Client.Implementations
{
    public abstract class ApiClientBase
    {
        protected readonly HttpClient _http;
        protected readonly JsonSerializerOptions _jsonOptions;

        protected ApiClientBase(HttpClient httpClient)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        protected void EnsureAuthorizationHeader()
        {
            var token = SessionManager.Instance.AccessToken;
            if (!string.IsNullOrWhiteSpace(token))
            {
                // Replace existing Authorization header if present
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _http.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}
