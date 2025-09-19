using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeAutomation.Core.Entities.Tasks;
using OfficeAutomation.Repositories.Base;

namespace OfficeAutomation.Repositories.Interfaces
{
    // Repository interface for managing TaskEntity records
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        // Retrieves all tasks assigned to a specific user
        Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(int userId);

        // Retrieves a task by its ID, including related details (e.g., assigned user)
        Task<TaskEntity?> GetTaskWithDetailsAsync(int taskId);
    }
}
