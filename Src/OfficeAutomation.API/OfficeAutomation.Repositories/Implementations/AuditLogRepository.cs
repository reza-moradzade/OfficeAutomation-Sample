using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Data.Context;
using OfficeAutomation.Repositories.Base;
using OfficeAutomation.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Implementations
{
    // Repository implementation for AuditLog entity, provides data access methods
    public class AuditLogRepository : RepositoryBase<AuditLog>, IAuditLogRepository
    {
        private readonly OfficeAutomationDbContext _context;

        // Constructor to inject DbContext
        public AuditLogRepository(OfficeAutomationDbContext context) : base(context)
        {
            _context = context;
        }

        // Retrieves all audit logs for a specific user
        public async Task<IEnumerable<AuditLog>> GetLogsByUserIdAsync(int userId)
        {
            return await _context.AuditLogs
                                 .Where(l => l.UserId == userId)
                                 .OrderByDescending(l => l.Timestamp)
                                 .ToListAsync();
        }

        // Retrieves all audit logs for a specific action
        public async Task<IEnumerable<AuditLog>> GetLogsByActionAsync(string action)
        {
            return await _context.AuditLogs
                                 .Where(l => l.Action == action)
                                 .OrderByDescending(l => l.Timestamp)
                                 .ToListAsync();
        }

        // Saves changes to the database context
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
