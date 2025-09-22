using OfficeAutomation.Core.Entities.Users;
using OfficeAutomation.Core.Entities.Security;
using System;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        // Retrieves a user by their username.
        Task<User?> GetByUserNameAsync(string userName);

        // Retrieves a user by their unique user ID.
        Task<User?> GetByIdAsync(int userId);

        // Validates whether the provided username and password match a stored user.
        Task<bool> ValidateUserCredentialsAsync(string userName, string password);

        // Creates a new user session and returns its unique identifier.
        Task<Guid> CreateSessionAsync(int userId, string clientType, string ipAddress, string deviceInfo);

        // Logs out a specific session by its session ID.
        Task LogoutAsync(Guid sessionId);

        // Logs out all sessions associated with a specific user.
        Task LogoutAllAsync(int userId);

        // Retrieves the active session for a user with a specific client type.
        Task<UserSession?> GetActiveSessionAsync(int userId, string clientType);

        // Checks if a session is currently active by session ID.
        Task<bool> IsSessionActiveAsync(Guid sessionId);

        // Retrieves a session by its unique session ID.
        Task<UserSession?> GetSessionByIdAsync(Guid sessionId);

        // Retrieves all roles associated with a specific user.
        Task<string[]> GetUserRolesAsync(int userId);

        // Adds a refresh token to the database for a user.
        Task AddRefreshTokenAsync(RefreshToken token);

        // Retrieves a refresh token entity by its token string.
        Task<RefreshToken?> GetRefreshTokenAsync(string token);

        // Revokes a specific refresh token and optionally links it to a replacement token.
        Task RevokeRefreshTokenAsync(string token, string replacedByToken = null, string revokedByIp = null);

        // Revokes all refresh tokens for a specific user.
        Task RevokeAllRefreshTokensForUserAsync(int userId);
    }
}
