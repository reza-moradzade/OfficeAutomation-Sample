namespace OfficeAutomation.Client.Models
{
    // Data transfer object for login requests
    public class LoginDto
    {
        // Username for authentication
        public string UserName { get; set; } = null!;

        // Password for authentication
        public string Password { get; set; } = null!;
    }
}
