using OfficeAutomation.Core.Entities.Cartables;
using OfficeAutomation.Core.Entities.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeAutomation.Core.Entities.Tasks
{
    // Represents a task assigned to a user, with tracking for creation and completion.
    public class TaskEntity
    {
        // Primary key of the task.
        public int TaskId { get; set; }

        // Detailed description of the task.
        public string? Description { get; set; }

        // User ID of who created the task.
        public int CreatedBy { get; set; }

        // User ID of who completed the task, if applicable.
        public int? CompletedBy { get; set; }

        // Current status of the task (default "Pending").
        public string Status { get; set; } = "Pending";

        // Due date of the task, if any.
        public DateTime? DueDate { get; set; }

        // Indicates if the task is soft-deleted.
        public bool IsDeleted { get; set; } = false;

        // Navigation property to the assigned user.
        [ForeignKey(nameof(AssignedTo))]
        public User? AssignedUser { get; set; }

        // Navigation property to the user who created the task.
        [ForeignKey(nameof(CreatedBy))]
        public User CreatedByUser { get; set; } = null!;

        // Navigation property to the user who completed the task.
        [ForeignKey(nameof(CompletedBy))]
        public User? CompletedByUser { get; set; }

        // Title of the task.
        public string Title { get; set; } = string.Empty;

        // Foreign key to the assigned user.
        public int? AssignedTo { get; set; }

        // Concurrency token for optimistic locking.
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
