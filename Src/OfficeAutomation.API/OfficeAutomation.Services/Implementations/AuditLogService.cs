using OfficeAutomation.Repositories.Interfaces;
using OfficeAutomation.Services.Interfaces;
using OfficeAutomation.Core.Entities.Security;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace OfficeAutomation.Services.Implementations
{
    public class AuditLogService : IAuditLogService
    {
        // Repository for accessing audit log data
        private readonly IAuditLogRepository _auditLogRepository;

        // Constructor with dependency injection
        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        // Retrieve all audit logs
        public async Task<IEnumerable<AuditLog>> GetAllLogsAsync()
        {
            return await _auditLogRepository.GetAllAsync();
        }

        // Retrieve a specific audit log by its ID
        public async Task<AuditLog?> GetLogByIdAsync(int logId)
        {
            return await _auditLogRepository.GetByIdAsync(logId);
        }

        // Create a new audit log entry
        public async Task<AuditLog> CreateLogAsync(AuditLog log)
        {
            await _auditLogRepository.AddAsync(log);
            await _auditLogRepository.SaveChangesAsync(); // Commit changes to database
            return log;
        }

        // Create and save an audit log with details
        public async Task<AuditLog> LogAsync(int? userId, string action, string? details = null, string? ipAddress = null)
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                Details = details,
                IPAddress = ipAddress,
                Timestamp = DateTime.UtcNow
            };

            await _auditLogRepository.AddAsync(log);
            await _auditLogRepository.SaveChangesAsync(); // Commit changes to database
            return log;
        }

        // Retrieve all logs for a specific user
        public async Task<IEnumerable<AuditLog>> GetLogsByUserIdAsync(int userId)
        {
            return await _auditLogRepository.GetLogsByUserIdAsync(userId);
        }

        // Retrieve all logs filtered by action
        public async Task<IEnumerable<AuditLog>> GetLogsByActionAsync(string action)
        {
            return await _auditLogRepository.GetLogsByActionAsync(action);
        }
    }
}
