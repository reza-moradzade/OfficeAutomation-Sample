using System.Security.Claims;

namespace OfficeAutomation.Services.Interfaces
{
    // Interface for authentication-related operations
    public interface IAuthService
    {
        // Perform login using username, password, captcha, client info, and device info
        Task<AuthResultDto> LoginAsync(
            string userName,
            string password,
            string captchaToken,
            string clientType,
            string ipAddress,
            string deviceInfo
        );

        // Refresh an existing JWT using a refresh token
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken, string ipAddress);

        // Logout a specific session and revoke its tokens
        Task LogoutAsync(Guid sessionId, string ipAddress);

        // Logout all sessions of a user and revoke all refresh tokens
        Task LogoutAllAsync(int userId, string ipAddress);

        // Validate a JWT token and return its claims principal
        ClaimsPrincipal ValidateToken(string token);
    }
}
