using System.Threading.Tasks;

namespace OfficeAutomation.MAUI.Services
{
    public interface ITokenService
    {
        Task SaveTokenAsync(string token, string refreshToken);
        Task<string?> GetTokenAsync();
        Task<string?> GetRefreshTokenAsync();
        Task RemoveTokensAsync();
    }

    public class TokenService : ITokenService
    {
        private const string TokenKey = "oa_token";
        private const string RefreshKey = "oa_refresh";

        // Save tokens securely
        public async Task SaveTokenAsync(string token, string refreshToken)
        {
            await SecureStorage.SetAsync(TokenKey, token ?? string.Empty);
            await SecureStorage.SetAsync(RefreshKey, refreshToken ?? string.Empty);
        }

        // Retrieve access token
        public async Task<string?> GetTokenAsync()
        {
            return await SecureStorage.GetAsync(TokenKey);
        }

        // Retrieve refresh token
        public async Task<string?> GetRefreshTokenAsync()
        {
            return await SecureStorage.GetAsync(RefreshKey);
        }

        // Remove tokens
        public Task RemoveTokensAsync()
        {
            SecureStorage.Remove(TokenKey);
            SecureStorage.Remove(RefreshKey);
            return Task.CompletedTask;
        }
    }
}
