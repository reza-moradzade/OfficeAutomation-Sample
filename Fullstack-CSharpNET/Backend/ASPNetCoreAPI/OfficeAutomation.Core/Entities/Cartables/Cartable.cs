using System;
using OfficeAutomation.Core.Entities.Users;
using OfficeAutomation.Core.Entities.Tasks;

namespace OfficeAutomation.Core.Entities.Cartables
{
    // Represents a cartable (inbox/outbox) entry for a user
    public class Cartable
    {
        // Unique identifier of the cartable entry
        public int CartableId { get; set; }

        // ID of the user who owns this cartable entry
        public int UserId { get; set; }

        // ID of the task associated with this cartable entry
        public int TaskId { get; set; }

        // Indicates whether the item has been read by the user
        public bool IsRead { get; set; } = false;

        // The timestamp when the item was received
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

        // Navigation property to the User entity
        public User User { get; set; } = null!;

        // RowVersion for concurrency control
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation property to the Task entity
        public TaskEntity Task { get; set; } = null!;
    }
}
