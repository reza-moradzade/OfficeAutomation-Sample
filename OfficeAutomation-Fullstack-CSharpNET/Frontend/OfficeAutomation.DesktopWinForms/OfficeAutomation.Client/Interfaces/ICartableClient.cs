using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeAutomation.Client.Models;

namespace OfficeAutomation.Client.Interfaces
{
    // Interface for Cartable API client
    public interface ICartableClient
    {
        // Get all cartable items for the current user
        Task<List<CartableItemDto>> GetAllAsync();
    }
}
