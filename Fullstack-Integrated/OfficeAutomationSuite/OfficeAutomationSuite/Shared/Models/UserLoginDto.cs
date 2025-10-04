namespace OfficeAutomationSuite.Shared.Models
{
    // DTO for user login data
    // Sent from client to server during login
    public class UserLoginDto
    {
        // Username of the user
        public string Username { get; set; }

        // Password of the user
        public string Password { get; set; }
    }
}
