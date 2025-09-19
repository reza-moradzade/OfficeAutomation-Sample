using OfficeAutomation.Core.Entities.Users;
using System;

namespace OfficeAutomation.Core.Entities.Security
{
    // Represents a refresh token used for extending user authentication sessions.
    public class RefreshToken
    {
        // Unique identifier for the refresh token.
        public Guid RefreshTokenId { get; set; }

        // Related user identifier.
        public int UserId { get; set; }

        // The token string value.
        public string Token { get; set; } = string.Empty;

        // Expiration date of the token.
        public DateTime ExpiresAt { get; set; }

        // Creation date of the token.
        public DateTime CreatedAt { get; set; }

        // IP address where the token was created.
        public string? CreatedByIp { get; set; }

        // Date when the token was revoked, if applicable.
        public DateTime? RevokedAt { get; set; }

        // IP address that revoked the token.
        public string? RevokedByIp { get; set; }

        // Token that replaced this one, if applicable.
        public string? ReplacedByToken { get; set; }

        // Indicates whether the token is revoked.
        public bool IsRevoked => RevokedAt.HasValue;

        // Navigation property to the related user.
        public User User { get; set; }
    }
}
