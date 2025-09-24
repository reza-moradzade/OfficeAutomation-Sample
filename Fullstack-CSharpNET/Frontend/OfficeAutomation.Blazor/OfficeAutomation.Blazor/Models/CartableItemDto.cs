using System;

namespace OfficeAutomation.Blazor.Models
{
    // Data Transfer Object representing a cartable item
    public class CartableItemDto
    {
        public int CartableId { get; set; }          // Unique ID of the cartable record
        public int TaskId { get; set; }              // Related task ID
        public string Title { get; set; } = "";      // Task title
        public string? Description { get; set; }     // Task description
        public string Status { get; set; } = "";     // Status (Pending, InProgress, Completed, etc.)
        public DateTime? DueDate { get; set; }       // Optional due date
        public bool IsRead { get; set; }             // Indicates whether the item has been read
        public DateTime ReceivedAt { get; set; }     // Date and time the item was received

        // Additional properties for displaying the assigned user
        public string? AssignedToUserName { get; set; } // Username of the assigned user
        public string? AssignedToFullName { get; set; } // Full name of the assigned user
    }
}
