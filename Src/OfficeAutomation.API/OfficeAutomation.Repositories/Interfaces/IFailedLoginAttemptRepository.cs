using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Repositories.Base;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Interfaces
{
    // Repository interface for managing failed login attempts
    public interface IFailedLoginAttemptRepository : IRepository<FailedLoginAttempt>
    {
        // Adds a new failed login attempt record
        Task AddAttemptAsync(FailedLoginAttempt attempt);

        // Counts the number of failed login attempts for a user within a specific time window (in minutes)
        Task<int> CountRecentAttemptsAsync(int userId, int minutes);
    }
}
