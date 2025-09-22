using System;
using System.Collections.Generic;

namespace OfficeAutomation.Client.Models
{
    // Data transfer object for authentication results
    public class AuthResultDto
    {
        public string UserName { get; set; } = null!;           // Username of the authenticated user
        public string? FullName { get; set; }                   // Full name of the user (optional)
        public string Token { get; set; } = null!;             // JWT access token
        public string? RefreshToken { get; set; }              // Optional refresh token
        public DateTime Expiration { get; set; }               // Token expiration date/time
        public List<string>? Roles { get; set; }               // User roles
        public bool IsLocked { get; set; }                     // Account locked flag
        public string? Message { get; set; }                   // Optional message (e.g., lock reason)
        public int LockRemainingSeconds { get; set; }          // Remaining lock time in seconds
        public int FailedLoginCount { get; set; }              // Number of failed login attempts
    }
}
