using System;

namespace OfficeAutomation.Client.Models
{
    public class CartableItemDto
    {
        public int CartableId { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string AssignedToUserName { get; set; } = null!;
        public string AssignedToFullName { get; set; } = null!;
    }
}
