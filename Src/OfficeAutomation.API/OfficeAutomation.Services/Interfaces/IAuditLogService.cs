using OfficeAutomation.Core.Entities.Security;

namespace OfficeAutomation.Services.Interfaces
{
    // Interface for Audit Log business operations
    public interface IAuditLogService
    {
        // Retrieve all audit logs
        Task<IEnumerable<AuditLog>> GetAllLogsAsync();

        // Retrieve a specific audit log by its ID
        Task<AuditLog?> GetLogByIdAsync(int logId);

        // Create a new audit log entry
        Task<AuditLog> CreateLogAsync(AuditLog log);

        // Log a specific action with optional details and IP address
        Task<AuditLog> LogAsync(int? userId, string action, string? details = null, string? ipAddress = null);

        // Retrieve all logs for a specific user
        Task<IEnumerable<AuditLog>> GetLogsByUserIdAsync(int userId);

        // Retrieve all logs filtered by action type
        Task<IEnumerable<AuditLog>> GetLogsByActionAsync(string action);
    }
}
