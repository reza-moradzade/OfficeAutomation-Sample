using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Repositories.Interfaces;
using OfficeAutomation.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace OfficeAutomation.Services.Implementations
{
    public class AuthService : IAuthService
    {
        // Repositories and services for authentication operations
        private readonly IAuthRepository _repository;
        private readonly IFailedLoginAttemptRepository _failedLoginRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IAuditLogService _auditLogService;
        private readonly IConfiguration _configuration;

        // Constructor with dependency injection
        public AuthService(
            IAuthRepository repository,
            IFailedLoginAttemptRepository failedLoginRepo,
            IRefreshTokenRepository refreshTokenRepo,
            IAuditLogService auditLogService,
            IConfiguration configuration
        )
        {
            _repository = repository;
            _failedLoginRepo = failedLoginRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _auditLogService = auditLogService;
            _configuration = configuration;
        }

        // Login method: validates captcha, credentials, email, manages sessions and refresh tokens
        public async Task<AuthResultDto> LoginAsync(
            string userName,
            string password,
            string captchaToken,
            string clientType,
            string ipAddress,
            string deviceInfo
        )
        {
            // Validate captcha token
            if (!await ValidateCaptchaAsync(captchaToken))
                throw new Exception("Invalid Captcha.");

            // Retrieve user by username
            var user = await _repository.GetByUserNameAsync(userName);
            if (user == null)
            {
                await _auditLogService.LogAsync(null, "LoginFailed", $"Username not found: {userName}", ipAddress);
                throw new Exception("Invalid username or password.");
            }

            // Check for temporary lockout due to failed attempts
            int lockoutMinutes = int.Parse(_configuration["Auth:LockoutMinutes"] ?? "15");
            int maxFailedAttempts = int.Parse(_configuration["Auth:MaxFailedAttempts"] ?? "5");

            int recentAttempts = await _failedLoginRepo.CountRecentAttemptsAsync(user.UserId, lockoutMinutes);
            if (recentAttempts >= maxFailedAttempts)
            {
                await _auditLogService.LogAsync(user.UserId, "AccountLocked",
                    $"Too many failed attempts in {lockoutMinutes} minutes", ipAddress);

                return new AuthResultDto
                {
                    message = $"Account locked after {maxFailedAttempts} failed login attempts. Please try again in {lockoutMinutes} minute(s).",
                    IsLocked = true,
                    LockRemainingSeconds = lockoutMinutes * 60,
                    failedLoginCount= maxFailedAttempts,
                };
            }

            // Validate user password
            if (!await _repository.ValidateUserCredentialsAsync(userName, password))
            {
                await _failedLoginRepo.AddAttemptAsync(new FailedLoginAttempt
                {
                    UserId = user.UserId,
                    IPAddress = ipAddress,
                    AttemptDate = DateTime.UtcNow
                });

                await _auditLogService.LogAsync(user.UserId, "LoginFailed", "Invalid password", ipAddress);
                throw new Exception("Invalid username or password.");
            }

            // Ensure email is confirmed
            if (!user.IsEmailConfirmed)
            {
                await _auditLogService.LogAsync(user.UserId, "LoginFailed", "Email not confirmed", ipAddress);
                throw new Exception("Email is not confirmed. Please verify your email.");
            }

            // Manage existing session and create new session
            var existingSession = await _repository.GetActiveSessionAsync(user.UserId, clientType);
            if (existingSession != null)
                await _repository.LogoutAsync(existingSession.SessionId);

            var sessionId = await _repository.CreateSessionAsync(user.UserId, clientType, ipAddress, deviceInfo);
            var accessToken = GenerateJwtToken(user.UserId, user.UserName);

            // Create a refresh token
            var refreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenDays"])),
                CreatedByIp = ipAddress
            };

            await _refreshTokenRepo.AddAsync(refreshToken);
            await _refreshTokenRepo.SaveChangesAsync();

            // Log successful login
            await _auditLogService.LogAsync(user.UserId, "LoginSuccess", $"SessionId: {sessionId}", ipAddress);

            return new AuthResultDto
            {
                UserName = user.UserName,
                FullName = user.FullName ?? "",
                Token = accessToken,
                RefreshToken = refreshToken.Token,
                Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationMinutes"])),
                Roles = await _repository.GetUserRolesAsync(user.UserId)
            };
        }

        // Validate JWT token and return ClaimsPrincipal
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, parameters, out var securityToken);
            if (securityToken is not JwtSecurityToken)
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        // Refresh an expired refresh token
        public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var storedToken = await _refreshTokenRepo.GetByTokenAsync(refreshToken);
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt <= DateTime.UtcNow)
            {
                await _auditLogService.LogAsync(null, "RefreshTokenFailed", $"Token invalid or expired: {refreshToken}", ipAddress);
                throw new Exception("Invalid or expired refresh token.");
            }

            var user = await _repository.GetByIdAsync(storedToken.UserId);
            if (user == null)
            {
                await _auditLogService.LogAsync(null, "RefreshTokenFailed", $"User not found for token: {refreshToken}", ipAddress);
                throw new Exception("User not found.");
            }

            await _refreshTokenRepo.RevokeAsync(storedToken.RefreshTokenId, ipAddress);

            var newRefreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenDays"])),
                CreatedByIp = ipAddress
            };

            await _refreshTokenRepo.AddAsync(newRefreshToken);
            await _refreshTokenRepo.SaveChangesAsync();

            var newAccessToken = GenerateJwtToken(user.UserId, user.UserName);

            await _auditLogService.LogAsync(user.UserId, "RefreshTokenSuccess", $"New token issued", ipAddress);

            return new AuthResultDto
            {
                UserName = user.UserName,
                FullName = user.FullName ?? "",
                Token = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationMinutes"])),
                Roles = await _repository.GetUserRolesAsync(user.UserId)
            };
        }

        // Logout a single session and revoke its refresh tokens
        public async Task LogoutAsync(Guid sessionId, string ipAddress)
        {
            var session = await _repository.GetSessionByIdAsync(sessionId);
            if (session != null)
            {
                await _repository.LogoutAsync(sessionId);
                await _refreshTokenRepo.RevokeAllAsync(session.UserId, ipAddress);
                await _refreshTokenRepo.SaveChangesAsync();

                await _auditLogService.LogAsync(session.UserId, "Logout", $"SessionId: {sessionId}", ipAddress);
            }
        }

        // Logout all sessions for a user and revoke all refresh tokens
        public async Task LogoutAllAsync(int userId, string ipAddress)
        {
            await _repository.LogoutAllAsync(userId);
            await _refreshTokenRepo.RevokeAllAsync(userId, ipAddress);
            await _refreshTokenRepo.SaveChangesAsync();

            await _auditLogService.LogAsync(userId, "LogoutAll", "All sessions revoked", ipAddress);
        }

        // Generate JWT access token for user
        private string GenerateJwtToken(int userId, string userName)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Simple placeholder method to validate captcha
        private async Task<bool> ValidateCaptchaAsync(string captchaToken)
        {
            await Task.Delay(10);
            return !string.IsNullOrEmpty(captchaToken);
        }
    }
}
