using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeAutomation.Core.Entities.Users;
using OfficeAutomation.Repositories.Base;

namespace OfficeAutomation.Repositories.Interfaces
{
    // Repository interface for managing UserSession entities
    public interface IUserSessionRepository : IRepository<UserSession>
    {
        // Retrieves the active session for a specific user and client type
        Task<UserSession?> GetActiveSessionAsync(int userId, string clientType);

        // Retrieves all sessions associated with a specific user
        Task<IEnumerable<UserSession>> GetSessionsByUserIdAsync(int userId);

        // Revokes (deactivates) a specific session by its unique identifier
        Task RevokeSessionAsync(Guid sessionId);
    }
}
