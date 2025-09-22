using OfficeAutomation.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Interfaces
{
    // Repository interface for managing cartable items
    public interface ICartableRepository
    {
        // Get all cartable items for a specific user, including joined task and user info
        Task<List<CartableItemDto>> GetUserCartableJoinedAsync(int userId);
    }
}
