using Microsoft.EntityFrameworkCore;
using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Data.Context;
using OfficeAutomation.Repositories.Base;
using OfficeAutomation.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Implementations
{
    public class FailedLoginAttemptRepository : RepositoryBase<FailedLoginAttempt>, IFailedLoginAttemptRepository
    {
        // DbContext for database operations
        private readonly OfficeAutomationDbContext _context;

        // Constructor with dependency injection of DbContext
        public FailedLoginAttemptRepository(OfficeAutomationDbContext context) : base(context)
        {
            _context = context;
        }

        // Add a failed login attempt to the database
        public async Task AddAttemptAsync(FailedLoginAttempt attempt)
        {
            if (attempt == null)
                throw new ArgumentNullException(nameof(attempt));

            // Ensure the attempt date is in UTC
            attempt.AttemptDate = DateTime.UtcNow;

            // Add the attempt to the DbSet
            await _context.FailedLoginAttempts.AddAsync(attempt);

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        // Count the number of recent failed login attempts for a user
        public async Task<int> CountRecentAttemptsAsync(int userId, int minutes)
        {
            // Calculate the threshold datetime
            var threshold = DateTime.UtcNow.AddMinutes(-minutes);

            // Query the DbSet for attempts within the threshold
            return await _context.FailedLoginAttempts
                                 .Where(a => a.UserId == userId && a.AttemptDate >= threshold)
                                 .CountAsync();
        }
    }
}
