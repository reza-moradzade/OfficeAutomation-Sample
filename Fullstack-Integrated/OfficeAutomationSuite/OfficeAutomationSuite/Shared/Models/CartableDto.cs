namespace OfficeAutomationSuite.Shared.Models
{
    // DTO for Cartable (task/assignment) data
    // Used to transfer Cartable info from server to client
    public class CartableDto
    {
        // Unique identifier of the cartable
        public int CartableId { get; set; }

        // Title of the cartable
        public string Title { get; set; }

        // Description or details of the cartable
        public string Description { get; set; }

        // Indicates if the cartable is completed
        public bool IsCompleted { get; set; }

        // Name of the user assigned to this cartable
        public string AssignedTo { get; set; }

        // Creation date of the cartable
        public DateTime CreatedAt { get; set; }
    }
}
