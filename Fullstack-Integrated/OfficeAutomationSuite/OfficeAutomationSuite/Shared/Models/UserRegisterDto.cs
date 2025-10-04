namespace OfficeAutomationSuite.Shared.Models
{
    // DTO for user registration data
    // Sent from client to server during user registration
    public class UserRegisterDto
    {
        // Username of the new user
        public string Username { get; set; }

        // Email address of the new user
        public string Email { get; set; }

        // Full name of the new user
        public string FullName { get; set; }

        // Password for the new user account
        public string Password { get; set; }
    }
}
