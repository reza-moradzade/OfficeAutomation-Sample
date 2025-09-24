using OfficeAutomation.Blazor.Models;

namespace OfficeAutomation.Blazor.Services.Interfaces
{
    // Defines authentication operations for the application
    public interface IAuthService
    {
        // Performs user login and returns authentication response
        Task<AuthResponse?> LoginAsync(string username, string password);

        // Performs user logout
        Task LogoutAsync();
    }
}
