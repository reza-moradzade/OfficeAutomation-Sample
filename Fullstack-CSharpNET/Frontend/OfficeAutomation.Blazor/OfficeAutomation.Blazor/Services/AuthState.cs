namespace OfficeAutomation.Blazor.Services
{
    // Holds the current authentication state of the user
    public class AuthState
    {
        public bool IsAuthenticated { get; set; } = false;  // Indicates whether the user is logged in
        public string? Token { get; set; }                  // Access token for API requests
        public string? UserName { get; set; }               // User's username
        public string? FullName { get; set; }               // User's full name
    }
}
