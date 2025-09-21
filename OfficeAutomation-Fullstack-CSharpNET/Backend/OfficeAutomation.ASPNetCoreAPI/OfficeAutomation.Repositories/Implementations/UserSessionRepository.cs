using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeAutomation.Core.Entities.Users;
using OfficeAutomation.Data.Context;
using OfficeAutomation.Repositories.Base;
using OfficeAutomation.Repositories.Interfaces;

namespace OfficeAutomation.Repositories.Implementations
{
    public class UserSessionRepository : RepositoryBase<UserSession>, IUserSessionRepository
    {
        // DbContext for database operations
        private readonly OfficeAutomationDbContext _context;

        // Constructor with dependency injection of DbContext
        public UserSessionRepository(OfficeAutomationDbContext context) : base(context)
        {
            _context = context;
        }

        // Get the active session for a specific user and client type
        public async Task<UserSession?> GetActiveSessionAsync(int userId, string clientType)
        {
            return await _context.UserSessions
                                 .Where(s => s.UserId == userId &&
                                             s.ClientType == clientType &&
                                             s.IsActive)
                                 .FirstOrDefaultAsync();
        }

        // Get all sessions for a specific user
        public async Task<IEnumerable<UserSession>> GetSessionsByUserIdAsync(int userId)
        {
            return await _context.UserSessions
                                 .Where(s => s.UserId == userId)
                                 .ToListAsync();
        }

        // Revoke a session by setting it inactive and updating the last activity
        public async Task RevokeSessionAsync(Guid sessionId)
        {
            var session = await _context.UserSessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
            if (session != null)
            {
                session.IsActive = false;
                session.LastActivity = DateTime.UtcNow;
                _context.UserSessions.Update(session);
            }
        }
    }
}
