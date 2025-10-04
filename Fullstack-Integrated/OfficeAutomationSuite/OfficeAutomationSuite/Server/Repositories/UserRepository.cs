using OfficeAutomationSuite.Server.Data;
using OfficeAutomationSuite.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace OfficeAutomationSuite.Server.Repositories
{
    // Repository for User entity operations
    // Handles database interactions related to users
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Add a new user to the database
        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Retrieve a user by username
        // Excludes users marked as deleted
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
        }
    }
}
