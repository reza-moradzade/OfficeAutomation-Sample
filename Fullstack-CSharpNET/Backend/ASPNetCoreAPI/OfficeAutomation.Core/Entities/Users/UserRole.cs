using System;

namespace OfficeAutomation.Core.Entities.Users
{
    // Represents the association between a user and a role.
    public class UserRole
    {
        // Foreign key to the user.
        public int UserId { get; set; }

        // Foreign key to the role.
        public int RoleId { get; set; }

        // Navigation property to the related user.
        public User User { get; set; } = null!;

        // Navigation property to the related role.
        public Role Role { get; set; } = null!;
    }
}
