namespace OfficeAutomation.MAUI.Services
{
    /// <summary>
    /// Holds the current authentication state of the user.
    /// This is registered as a Singleton service in MauiProgram.cs
    /// so it can be injected anywhere in the app (e.g., pages, services).
    /// </summary>
    public class AuthState
    {
        /// <summary>
        /// Indicates whether the user is currently authenticated.
        /// </summary>
        public bool IsAuthenticated { get; set; } = false;

        /// <summary>
        /// The access token received from the API (used for authorized requests).
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// The username of the logged-in user.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// The full name of the logged-in user (optional, based on API response).
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Clears all authentication-related properties (use on logout).
        /// </summary>
        public void Clear()
        {
            IsAuthenticated = false;
            Token = null;
            UserName = null;
            FullName = null;
        }
    }
}
