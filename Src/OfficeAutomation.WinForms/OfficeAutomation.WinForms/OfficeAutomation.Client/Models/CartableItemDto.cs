using System;

namespace OfficeAutomation.Client.Models
{
    // Data transfer object for cartable items
    public class CartableItemDto
    {
        public int CartableId { get; set; }            // Unique cartable item ID
        public int TaskId { get; set; }                // Related task ID
        public string Title { get; set; } = null!;     // Title of the item
        public string? Description { get; set; }       // Optional description
        public string Status { get; set; } = null!;    // Current status of the item
        public DateTime? DueDate { get; set; }         // Optional due date
        public bool IsRead { get; set; }               // Read/unread flag
        public DateTime ReceivedAt { get; set; }       // Date/time when received
        public string AssignedToUserName { get; set; } = null!;  // Username of the assignee
        public string AssignedToFullName { get; set; } = null!;  // Full name of the assignee
    }
}
