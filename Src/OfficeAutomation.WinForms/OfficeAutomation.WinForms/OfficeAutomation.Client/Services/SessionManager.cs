using System;
using System.Collections.Generic;
using OfficeAutomation.Client.Models;

namespace OfficeAutomation.Client.Services
{
    // Singleton for holding current authenticated session info (in-memory)
    public class SessionManager
    {
        private static readonly Lazy<SessionManager> _lazy = new Lazy<SessionManager>(() => new SessionManager());
        public static SessionManager Instance => _lazy.Value;

        // Private constructor for singleton
        private SessionManager() { }

        // Access and refresh tokens
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }

        // Token expiration
        public DateTime? Expiration { get; private set; }

        // User info
        public string? UserName { get; private set; }
        public string? FullName { get; private set; }

        // Roles assigned to the user
        public List<string> Roles { get; private set; } = new List<string>();

        // Indicates if the session is currently authenticated
        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken) && Expiration.HasValue && Expiration.Value > DateTime.UtcNow;

        // Initialize session from AuthResultDto
        public void SetSession(AuthResultDto auth)
        {
            if (auth == null) throw new ArgumentNullException(nameof(auth));
            AccessToken = auth.Token;
            RefreshToken = auth.RefreshToken;
            Expiration = auth.Expiration.ToUniversalTime();
            UserName = auth.UserName;
            FullName = auth.FullName;
            Roles = auth.Roles ?? new List<string>();
        }

        // Clear session info
        public void Clear()
        {
            AccessToken = null;
            RefreshToken = null;
            Expiration = null;
            UserName = null;
            FullName = null;
            Roles.Clear();
        }
    }
}
