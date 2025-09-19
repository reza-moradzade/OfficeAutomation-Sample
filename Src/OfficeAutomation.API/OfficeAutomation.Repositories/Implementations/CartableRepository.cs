using Microsoft.EntityFrameworkCore;
using OfficeAutomation.Core.DTOs;
using OfficeAutomation.Data.Context;
using OfficeAutomation.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeAutomation.Repositories.Implementations
{
    // Repository for Cartable entity operations
    public class CartableRepository : ICartableRepository
    {
        // EF Core DbContext for database access
        private readonly OfficeAutomationDbContext _ctx;

        // Constructor with dependency injection
        public CartableRepository(OfficeAutomationDbContext ctx) => _ctx = ctx;

        // ======================
        // Get a user's cartable items joined with related Task and assigned User info
        // ======================
        public async Task<List<CartableItemDto>> GetUserCartableJoinedAsync(int userId)
        {
            var items = await _ctx.Cartable
                .AsNoTracking() // Do not track changes for performance optimization
                .Where(c => c.UserId == userId) // Filter cartable items for the specified user
                .Join(_ctx.Tasks, c => c.TaskId, t => t.TaskId, (c, t) => new { c, t }) // Join with Tasks table
                .Join(_ctx.Users, ct => ct.t.AssignedTo, u => u.UserId, (ct, u) => new { ct.c, ct.t, u }) // Join with assigned user
                .OrderByDescending(x => x.c.ReceivedAt) // Order by ReceivedAt descending
                .Select(x => new CartableItemDto // Map to DTO for service/controller
                {
                    CartableId = x.c.CartableId,
                    TaskId = x.c.TaskId,
                    Title = x.t.Title,
                    Description = x.t.Description,
                    Status = x.t.Status,
                    DueDate = x.t.DueDate,
                    IsRead = x.c.IsRead,
                    ReceivedAt = x.c.ReceivedAt,
                    AssignedToUserName = x.u.UserName,
                    AssignedToFullName = x.u.FullName
                })
                .ToListAsync();

            return items;
        }
    }
}
