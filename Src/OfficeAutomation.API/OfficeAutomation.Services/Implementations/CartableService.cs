using OfficeAutomation.Core.DTOs;
using OfficeAutomation.Repositories.Interfaces;
using OfficeAutomation.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfficeAutomation.Services.Implementations
{
    public class CartableService : ICartableService
    {
        // Repository to access cartable data
        private readonly ICartableRepository _repo;

        // Constructor with dependency injection
        public CartableService(ICartableRepository repo)
        {
            _repo = repo;
        }

        // Retrieve cartable items for a specific user
        public Task<List<CartableItemDto>> GetMyCartableAsync(int userId)
        {
            return _repo.GetUserCartableJoinedAsync(userId);
        }
    }
}
