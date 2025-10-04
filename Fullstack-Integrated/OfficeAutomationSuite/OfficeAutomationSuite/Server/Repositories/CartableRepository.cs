using Microsoft.EntityFrameworkCore;
using OfficeAutomationSuite.Server.Data;
using OfficeAutomationSuite.Server.Data.Entities;

namespace OfficeAutomationSuite.Server.Repositories
{
    // Repository for Cartable entity operations
    // Handles database interactions related to cartables (tasks/assignments)
    public class CartableRepository
    {
        private readonly AppDbContext _context;

        public CartableRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all cartables including their assigned users
        public async Task<List<Cartable>> GetAllAsync()
        {
            return await _context.Cartables
                                 .Include(c => c.AssignedUser) // load related user
                                 .ToListAsync();
        }
    }
}
