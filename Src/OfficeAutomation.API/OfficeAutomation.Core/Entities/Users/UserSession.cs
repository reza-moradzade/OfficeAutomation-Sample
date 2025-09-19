using System;

namespace OfficeAutomation.Core.Entities.Users
{
    // Represents a user's active session in the system.
    public class UserSession
    {
        // Unique identifier for the session.
        public Guid SessionId { get; set; } = Guid.NewGuid();

        // Foreign key to the associated user.
        public int UserId { get; set; }

        // Type of client (e.g., web, mobile, desktop).
        public string ClientType { get; set; } = null!;

        // Timestamp of the last activity in this session.
        public DateTime LastActivity { get; set; } = DateTime.UtcNow;

        // Indicates whether the session is active.
        public bool IsActive { get; set; } = true;

        // Navigation property to the related user.
        public User User { get; set; } = null!;
    }
}
