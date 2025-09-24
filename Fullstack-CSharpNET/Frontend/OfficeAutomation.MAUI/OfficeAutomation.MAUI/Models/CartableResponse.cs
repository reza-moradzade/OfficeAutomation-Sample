using System;

namespace OfficeAutomation.MAUI.Models
{
    /// <summary>
    /// Represents a response item in the cartable system, returned by the API.
    /// </summary>
    public class CartableResponse
    {
        public int CartableId { get; set; }                 // Unique ID of the cartable item
        public int TaskId { get; set; }                     // Related task ID
        public string Title { get; set; } = string.Empty;   // Title of the cartable item
        public string? Description { get; set; }           // Optional description
        public string Status { get; set; } = string.Empty; // Current status (e.g., Pending, Completed)
        public DateTime? DueDate { get; set; }             // Optional due date
        public bool IsRead { get; set; }                   // Indicates whether the item has been read
        public DateTime ReceivedAt { get; set; }           // Date and time the item was received
    }
}
