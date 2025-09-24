using System;
using System.Collections.Generic;

namespace OfficeAutomation.MAUI.Models
{
    /// <summary>
    /// Represents the response returned after a successful authentication.
    /// </summary>
    public class AuthResponse
    {
        public string UserName { get; set; } = string.Empty;    // Username of the authenticated user
        public string FullName { get; set; } = string.Empty;    // Full name of the user
        public string Token { get; set; } = string.Empty;       // Access token for API calls
        public string Expiration { get; set; } = string.Empty;  // Token expiration datetime as string
        public List<string> Roles { get; set; } = new();        // List of user roles
        public string RefreshToken { get; set; } = string.Empty;// Refresh token for renewing access token
    }
}
