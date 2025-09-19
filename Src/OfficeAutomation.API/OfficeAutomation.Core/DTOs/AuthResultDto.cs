// DTO for authentication result returned to the client after login or token refresh
public class AuthResultDto
{
    // Username of the authenticated user
    public string UserName { get; set; } = null!;

    // Full name of the authenticated user (optional)
    public string? FullName { get; set; }

    // JWT access token issued to the user
    public string Token { get; set; } = null!;

    // Refresh token issued to the user for renewing the access token
    public string RefreshToken { get; set; } = null!;

    // Expiration date and time of the access token (UTC)
    public DateTime Expiration { get; set; }

    // Roles assigned to the authenticated user
    public string[] Roles { get; set; } = Array.Empty<string>();

    // Indicates if the account is temporarily locked due to failed login attempts
    public bool IsLocked { get; set; } = false;

    // Remaining lockout time in seconds if the account is locked
    public int LockRemainingSeconds { get; set; } = 0;
}
