using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace OfficeAutomation.Blazor.Services
{
    public interface ITokenService
    {
        Task SaveTokenAsync(string token, string refreshToken);
        Task<string> GetTokenAsync();
        Task<string> GetRefreshTokenAsync();
        Task RemoveTokensAsync();
    }

    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _js;
        private const string TokenKey = "oa_token";
        private const string RefreshKey = "oa_refresh";

        public TokenService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SaveTokenAsync(string token, string refreshToken)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token ?? string.Empty);
            await _js.InvokeVoidAsync("localStorage.setItem", RefreshKey, refreshToken ?? string.Empty);
        }

        public async Task<string> GetTokenAsync() =>
            await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);

        public async Task<string> GetRefreshTokenAsync() =>
            await _js.InvokeAsync<string>("localStorage.getItem", RefreshKey);

        public async Task RemoveTokensAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", RefreshKey);
        }
    }
}
