using System.Collections.Generic;

namespace OfficeAutomation.Blazor.Models
{
    // Represents the response returned after a successful authentication
    public class AuthResponse
    {
        public string UserName { get; set; }         // Username of the authenticated user
        public string FullName { get; set; }         // Full name of the user
        public string Token { get; set; }            // Access token for API calls
        public string Expiration { get; set; }       // Token expiration datetime (string format)
        public List<string> Roles { get; set; }      // List of user roles
        public string RefreshToken { get; set; }     // Refresh token for renewing the access token
    }
}
