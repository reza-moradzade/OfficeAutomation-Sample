// Unit of Work interface for managing multiple repositories and committing changes as a single transaction
using System;
using System.Threading.Tasks;
using OfficeAutomation.Repositories.Interfaces;

namespace OfficeAutomation.Repositories.Base
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }           // Repository for Task entities
        ICartableRepository Cartables { get; }   // Repository for Cartable entities
        IAuthRepository Auth { get; }            // Repository for authentication and user management
        IUserSessionRepository UserSessions { get; } // Repository for managing user sessions
        Task<int> CompleteAsync();               // Commit all changes in a single transaction
    }
}
