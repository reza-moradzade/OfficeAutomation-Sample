using OfficeAutomation.MAUI.Models;

namespace OfficeAutomation.MAUI.Services.Interfaces
{
    /// <summary>
    /// Defines authentication operations for the application.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Performs user login and returns authentication response.
        /// </summary>
        Task<AuthResponse?> LoginAsync(string username, string password);

        /// <summary>
        /// Performs user logout (clears tokens and state).
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Checks if a valid user session exists.
        /// </summary>
        Task<bool> IsAuthenticatedAsync();
    }
}
