using OfficeAutomationSuite.Server.Repositories;
using OfficeAutomationSuite.Shared.Models;

namespace OfficeAutomationSuite.Server.Services
{
    // Service for handling Cartable (task/assignment) operations
    public class CartableService
    {
        private readonly CartableRepository _repository;

        public CartableService(CartableRepository repository)
        {
            _repository = repository;
        }

        // Get all cartables and map to DTOs for client consumption
        public async Task<List<CartableDto>> GetAllCartablesAsync()
        {
            var cartables = await _repository.GetAllAsync();

            return cartables.Select(c => new CartableDto
            {
                CartableId = c.CartableId,
                Title = c.Title,
                Description = c.Description,
                IsCompleted = c.IsCompleted,
                AssignedTo = c.AssignedUser?.FullName ?? c.AssignedTo,
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }
}
