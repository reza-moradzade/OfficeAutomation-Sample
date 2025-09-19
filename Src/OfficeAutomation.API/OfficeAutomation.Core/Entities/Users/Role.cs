using System;
using System.Collections.Generic;

namespace OfficeAutomation.Core.Entities.Users
{
    // Represents a user role in the system.
    public class Role
    {
        // Primary key of the role.
        public int RoleId { get; set; }

        // Collection of user-role assignments associated with this role.
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
