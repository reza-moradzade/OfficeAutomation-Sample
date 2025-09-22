using System;

namespace OfficeAutomation.Core.DTOs
{
    // DTO representing an item in a user's cartable (inbox/outbox)
    public class CartableItemDto
    {
        // Unique identifier of the cartable entry
        public int CartableId { get; set; }

        // Task identifier associated with this cartable item
        public int TaskId { get; set; }

        // Title or subject of the cartable item
        public string Title { get; set; } = string.Empty;

        // Detailed description or notes about the item (optional)
        public string? Description { get; set; }

        // Current status of the task (e.g., Pending, InProgress, Completed)
        public string Status { get; set; } = string.Empty;

        // Due date of the task, if applicable
        public DateTime? DueDate { get; set; }

        // Indicates whether the item has been read by the user
        public bool IsRead { get; set; }

        // Timestamp when the item was received in the cartable
        public DateTime ReceivedAt { get; set; }

        // Username of the person to whom this task/item is assigned (optional)
        public string? AssignedToUserName { get; set; }

        // Full display name of the assignee (optional)
        public string? AssignedToFullName { get; set; }
    }
}
