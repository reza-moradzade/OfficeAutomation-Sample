using System;
using System.Threading.Tasks;
using OfficeAutomation.Core.Entities.Security;

namespace OfficeAutomation.Repositories.Interfaces
{
    // Repository interface for managing refresh tokens
    public interface IRefreshTokenRepository
    {
        // Adds a new refresh token to the database
        Task AddAsync(RefreshToken token);

        // Retrieves a refresh token by its token string
        Task<RefreshToken?> GetByTokenAsync(string token);

        // Revokes a specific refresh token, optionally linking it to a replacement token
        Task RevokeAsync(Guid refreshTokenId, string revokedByIp, string? replacedByToken = null);

        // Revokes all active refresh tokens for a specific user
        Task RevokeAllAsync(int userId, string revokedByIp);

        // Persists all pending changes to the database
        Task SaveChangesAsync();
    }
}
