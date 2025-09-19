using Microsoft.EntityFrameworkCore;
using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Data.Context;
using OfficeAutomation.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Implementations
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        // DbContext for database operations
        private readonly OfficeAutomationDbContext _context;

        // Constructor with dependency injection of DbContext
        public RefreshTokenRepository(OfficeAutomationDbContext context)
        {
            _context = context;
        }

        // Add a new refresh token to the database
        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
        }

        // Retrieve a refresh token by its token string
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        // Revoke a specific refresh token by its ID
        public async Task RevokeAsync(Guid refreshTokenId, string revokedByIp, string? replacedByToken = null)
        {
            var token = await _context.RefreshTokens.FindAsync(refreshTokenId);
            if (token != null && !token.IsRevoked)
            {
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIp = revokedByIp;
                token.ReplacedByToken = replacedByToken;
            }
        }

        // Revoke all active refresh tokens for a specific user
        public async Task RevokeAllAsync(int userId, string revokedByIp)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIp = revokedByIp;
            }
        }

        // Save all pending changes to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
