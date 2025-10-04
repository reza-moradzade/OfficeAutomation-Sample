using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeAutomationSuite.Server.Data.Entities
{
    // Cartable entity representing a task/assignment
    public class Cartable
    {
        [Key]
        public int CartableId { get; set; }  // Primary key

        [Required, MaxLength(200)]
        public string Title { get; set; }  // Task title

        [Required, MaxLength(1000)]
        public string Description { get; set; }  // Task description

        [Required]
        public int AssignedToUserId { get; set; }  // FK to User

        public bool IsCompleted { get; set; } = false;  // Completion status

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Creation timestamp

        // Navigation property to assigned User
        [ForeignKey("AssignedToUserId")]
        public User AssignedUser { get; set; }

        // Optional: store assigned user's name directly
        [MaxLength(100)]
        public string AssignedTo { get; set; }
    }
}
