using System;

namespace OfficeAutomation.Blazor.Models
{
    public class CartableResponse
    {
        public int CartableId { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}
