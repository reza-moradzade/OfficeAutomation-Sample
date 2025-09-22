using OfficeAutomation.Core.DTOs;

namespace OfficeAutomation.Services.Interfaces
{
    // Interface for Cartable-related business logic
    public interface ICartableService
    {
        // Retrieve the cartable items for a specific user
        Task<List<CartableItemDto>> GetMyCartableAsync(int userId);
    }
}
