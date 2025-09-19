using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Interfaces
{
    // Repository interface for managing AuditLog entities
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        // Get all audit logs for a specific user
        Task<IEnumerable<AuditLog>> GetLogsByUserIdAsync(int userId);

        // Get all audit logs for a specific action
        Task<IEnumerable<AuditLog>> GetLogsByActionAsync(string action);

        // Save changes to the database
        Task SaveChangesAsync();
    }
}
