using System;
using System.Collections.Generic;
using OfficeAutomation.Client.Models;

namespace OfficeAutomation.Client.Services
{
    /// <summary>
    /// Singleton for holding current authenticated session info (in-memory).
    /// </summary>
    public class SessionManager
    {
        private static readonly Lazy<SessionManager> _lazy = new Lazy<SessionManager>(() => new SessionManager());
        public static SessionManager Instance => _lazy.Value;

        private SessionManager() { }

        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? Expiration { get; private set; }
        public string? UserName { get; private set; }
        public string? FullName { get; private set; }
        public List<string> Roles { get; private set; } = new List<string>();

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken) && Expiration.HasValue && Expiration.Value > DateTime.UtcNow;

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
