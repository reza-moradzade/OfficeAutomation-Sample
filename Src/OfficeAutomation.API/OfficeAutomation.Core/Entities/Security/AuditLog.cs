using OfficeAutomation.Core.Entities.Users;
using System;

namespace OfficeAutomation.Core.Entities.Security
{
    // Represents an audit log entry for tracking user actions.
    public class AuditLog
    {
        // Primary key for the log entry.
        public long LogId { get; set; }

        // Optional foreign key referencing the user who performed the action.
        public int? UserId { get; set; }

        // The type of action performed (e.g., Login, CreateTask, Delete).
        public string Action { get; set; } = null!;

        // Additional details or description of the action.
        public string? Details { get; set; }

        // IP address from which the action was performed.
        public string? IPAddress { get; set; }

        // Timestamp of when the action occurred (UTC).
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Navigation property to the related user entity.
        public User? User { get; set; }
    }
}
