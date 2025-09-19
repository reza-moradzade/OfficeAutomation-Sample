using System;
using System.ComponentModel.DataAnnotations;

namespace OfficeAutomation.Core.DTOs
{
    // DTO used for user login requests with validation attributes
    public class LoginDto
    {
        // Username of the user attempting to log in
        [Required(ErrorMessage = "UserName is required.")]
        [StringLength(50, ErrorMessage = "UserName cannot exceed 50 characters.")]
        public string UserName { get; set; } = null!;

        // Password of the user attempting to log in
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; } = null!;
    }
}
