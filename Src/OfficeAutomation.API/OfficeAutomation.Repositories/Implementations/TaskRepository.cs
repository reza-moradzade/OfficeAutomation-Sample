using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeAutomation.Core.Entities.Tasks;
using OfficeAutomation.Data.Context;
using OfficeAutomation.Repositories.Base;
using OfficeAutomation.Repositories.Interfaces;

namespace OfficeAutomation.Repositories.Implementations
{
    public class TaskRepository : RepositoryBase<TaskEntity>, ITaskRepository
    {
        // DbContext for database operations
        private readonly OfficeAutomationDbContext _context;

        // Constructor with dependency injection of DbContext
        public TaskRepository(OfficeAutomationDbContext context) : base(context)
        {
            _context = context;
        }

        // Retrieve all tasks assigned to a specific user
        public async Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks
                                 .Where(t => t.AssignedTo == userId && !t.IsDeleted)
                                 .Include(t => t.AssignedUser) // Include related user entity
                                 .ToListAsync();
        }

        // Retrieve a specific task with details, including assigned user
        public async Task<TaskEntity?> GetTaskWithDetailsAsync(int taskId)
        {
            return await _context.Tasks
                                 .Include(t => t.AssignedUser) // Include related user entity
                                 .FirstOrDefaultAsync(t => t.TaskId == taskId && !t.IsDeleted);
        }
    }
}
