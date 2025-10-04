using System;
using System.ComponentModel.DataAnnotations;

namespace OfficeAutomationSuite.Server.Data.Entities
{
    // User entity representing a user in the system
    public class User
    {
        [Key]
        public int UserId { get; set; }  // Primary key

        [Required, MaxLength(50)]
        public string Username { get; set; }  // Unique username

        [Required]
        public string PasswordHash { get; set; }  // Hashed password

        [Required]
        public string Salt { get; set; }  // Salt for password hashing

        [Required, MaxLength(100)]
        public string Email { get; set; }  // User email

        [MaxLength(100)]
        public string FullName { get; set; }  // Full name

        public bool IsActive { get; set; } = true;  // Is user active
        public bool IsDeleted { get; set; } = false;  // Soft delete flag
        public bool IsEmailConfirmed { get; set; } = false;  // Email confirmation status

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Creation timestamp
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // Last update timestamp
    }
}
