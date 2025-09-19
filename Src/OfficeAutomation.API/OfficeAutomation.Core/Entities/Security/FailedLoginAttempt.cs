using OfficeAutomation.Core.Entities.Users;
using System;

namespace OfficeAutomation.Core.Entities.Security
{
    // Represents a failed login attempt for a user
    public class FailedLoginAttempt
    {
        // Primary key
        public int AttemptId { get; set; }

        // User ID who attempted login
        public int UserId { get; set; }

        // Time of the failed attempt (UTC)
        public DateTime AttemptDate { get; set; } = DateTime.UtcNow;

        // IP address of the attempt (optional)
        public string? IPAddress { get; set; }

        // Navigation to the user
        public User? User { get; set; }
    }
}
