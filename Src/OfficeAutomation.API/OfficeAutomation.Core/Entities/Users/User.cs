using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Core.Entities.Tasks;
using OfficeAutomation.Core.Entities.Users;
using System;
using System.Collections.Generic;

namespace OfficeAutomation.Core.Entities.Users
{
    // Represents a system user with authentication and related entities.
    public class User
    {
        // Primary key of the user.
        public int UserId { get; set; }

        // Unique username for login.
        public string UserName { get; set; } = null!;

        // Full name of the user.
        public string? FullName { get; set; }

        // Indicates whether the user's email is confirmed.
        public bool IsEmailConfirmed { get; set; } = false;

        // Hashed password.
        public byte[] PasswordHash { get; set; } = null!;

        // Salt used for password hashing.
        public byte[] PasswordSalt { get; set; } = null!;

        // Concurrency token for optimistic locking.
        public byte[] RowVersion { get; set; } = null!;

        // Collection of failed login attempts for the user.
        public ICollection<FailedLoginAttempt> FailedLoginAttempts { get; set; } = new List<FailedLoginAttempt>();

        // Collection of active and historical user sessions.
        public ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();

        // Collection of tasks assigned or created by the user.
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();

        // Collection of roles assigned to the user.
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
