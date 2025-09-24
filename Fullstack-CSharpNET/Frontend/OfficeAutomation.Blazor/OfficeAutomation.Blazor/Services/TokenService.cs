using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace OfficeAutomation.Blazor.Services
{
    // Interface for token management
    public interface ITokenService
    {
        Task SaveTokenAsync(string token, string refreshToken);  // Save access and refresh tokens
        Task<string> GetTokenAsync();                             // Retrieve access token
        Task<string> GetRefreshTokenAsync();                      // Retrieve refresh token
        Task RemoveTokensAsync();                                 // Remove both tokens from storage
    }

    // Implementation of token management using browser localStorage
    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _js;
        private const string TokenKey = "oa_token";     // Key for access token in localStorage
        private const string RefreshKey = "oa_refresh"; // Key for refresh token in localStorage

        public TokenService(IJSRuntime js)
        {
            _js = js;
        }

        // Save tokens to localStorage
        public async Task SaveTokenAsync(string token, string refreshToken)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token ?? string.Empty);
            await _js.InvokeVoidAsync("localStorage.setItem", RefreshKey, refreshToken ?? string.Empty);
        }

        // Get access token from localStorage
        public async Task<string> GetTokenAsync() =>
            await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);

        // Get refresh token from localStorage
        public async Task<string> GetRefreshTokenAsync() =>
            await _js.InvokeAsync<string>("localStorage.getItem", RefreshKey);

        // Remove both access and refresh tokens from localStorage
        public async Task RemoveTokensAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", RefreshKey);
        }
    }
}
