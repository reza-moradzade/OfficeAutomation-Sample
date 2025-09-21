using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeAutomation.Core.Entities.Users;
using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace OfficeAutomation.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        // Stores the database connection string
        private readonly string _connectionString;

        // ======================
        // Constructor
        // ======================
        // Initializes the repository with the connection string from configuration
        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OfficeAutomationDB");
        }

        // ======================
        // Users
        // ======================
        // Retrieves a user by their username if the account is active and not deleted
        public async Task<User?> GetByUserNameAsync(string userName)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT *
                FROM Users
                WHERE UserName = @UserName
                  AND IsActive = 1
                  AND IsDeleted = 0;
            ";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = userName });
        }

        // Retrieves a user by their ID if the account is active and not deleted
        public async Task<User?> GetByIdAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT *
                FROM Users
                WHERE UserId = @UserId
                  AND IsActive = 1
                  AND IsDeleted = 0;
            ";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
        }

        // Validates user credentials by comparing the hashed password with the stored hash
        public async Task<bool> ValidateUserCredentialsAsync(string userName, string password)
        {
            var user = await GetByUserNameAsync(userName);
            if (user == null)
                return false;

            using var sha = SHA256.Create();
            var saltString = Encoding.UTF8.GetString(user.PasswordSalt);
            var combined = Encoding.UTF8.GetBytes(password + saltString);
            var hash = sha.ComputeHash(combined);

            return hash.SequenceEqual(user.PasswordHash);
        }

        // ======================
        // Sessions
        // ======================
        // Creates a new session for a user and deactivates any previous active session for the same client type
        public async Task<Guid> CreateSessionAsync(int userId, string clientType, string ipAddress, string deviceInfo)
        {
            using var connection = new SqlConnection(_connectionString);

            var activeSession = await GetActiveSessionAsync(userId, clientType);
            if (activeSession != null)
            {
                // Deactivate existing active session
                var deactivateSql = @"
                    UPDATE UserSessions
                    SET IsActive = 0,
                        LastActivity = SYSUTCDATETIME()
                    WHERE SessionId = @SessionId;
                ";
                await connection.ExecuteAsync(deactivateSql, new { SessionId = activeSession.SessionId });
            }

            var sessionId = Guid.NewGuid();
            var sql = @"
                INSERT INTO UserSessions(SessionId, UserId, ClientType, IPAddress, DeviceInfo, CreatedAt, LastActivity, IsActive)
                VALUES (@SessionId, @UserId, @ClientType, @IPAddress, @DeviceInfo, SYSUTCDATETIME(), SYSUTCDATETIME(), 1);
            ";
            await connection.ExecuteAsync(sql, new
            {
                SessionId = sessionId,
                UserId = userId,
                ClientType = clientType,
                IPAddress = ipAddress,
                DeviceInfo = deviceInfo
            });

            return sessionId;
        }

        // Deactivates a specific session and revokes all refresh tokens associated with that session
        public async Task LogoutAsync(Guid sessionId)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
                UPDATE UserSessions
                SET IsActive = 0,
                    LastActivity = SYSUTCDATETIME()
                WHERE SessionId = @SessionId;
            ";
            await connection.ExecuteAsync(sql, new { SessionId = sessionId });

            // Get UserId from session to revoke related refresh tokens
            var userIdSql = "SELECT UserId FROM UserSessions WHERE SessionId = @SessionId;";
            var userId = await connection.ExecuteScalarAsync<int?>(userIdSql, new { SessionId = sessionId });

            if (userId.HasValue)
            {
                var revokeSql = @"
                    UPDATE RefreshTokens
                    SET RevokedAt = SYSUTCDATETIME()
                    WHERE UserId = @UserId
                      AND RevokedAt IS NULL;
                ";
                await connection.ExecuteAsync(revokeSql, new { UserId = userId.Value });
            }
        }

        // Deactivates all sessions for a user and revokes all their refresh tokens
        public async Task LogoutAllAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);

            var sqlSessions = @"
                UPDATE UserSessions
                SET IsActive = 0,
                    LastActivity = SYSUTCDATETIME()
                WHERE UserId = @UserId;
            ";
            await connection.ExecuteAsync(sqlSessions, new { UserId = userId });

            var sqlTokens = @"
                UPDATE RefreshTokens
                SET RevokedAt = SYSUTCDATETIME(),
                    IsRevoked = 1
                WHERE UserId = @UserId
                  AND RevokedAt IS NULL;
            ";
            await connection.ExecuteAsync(sqlTokens, new { UserId = userId });
        }

        // Retrieves the active session for a user and client type
        public async Task<UserSession?> GetActiveSessionAsync(int userId, string clientType)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT TOP 1 *
                FROM UserSessions
                WHERE UserId = @UserId
                  AND ClientType = @ClientType
                  AND IsActive = 1;
            ";
            return await connection.QueryFirstOrDefaultAsync<UserSession>(sql, new { UserId = userId, ClientType = clientType });
        }

        // Retrieves a session by its unique ID
        public async Task<UserSession?> GetSessionByIdAsync(Guid sessionId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT *
                FROM UserSessions
                WHERE SessionId = @SessionId;
            ";
            return await connection.QueryFirstOrDefaultAsync<UserSession>(sql, new { SessionId = sessionId });
        }

        // Checks whether a session is currently active
        public async Task<bool> IsSessionActiveAsync(Guid sessionId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT COUNT(1)
                FROM UserSessions
                WHERE SessionId = @SessionId
                  AND IsActive = 1;
            ";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { SessionId = sessionId });
            return count > 0;
        }

        // ======================
        // Roles
        // ======================
        // Retrieves all role names associated with a specific user
        public async Task<string[]> GetUserRolesAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT r.RoleName
                FROM Roles r
                INNER JOIN UserRoles ur ON ur.RoleId = r.RoleId
                WHERE ur.UserId = @UserId;
            ";
            var roles = await connection.QueryAsync<string>(sql, new { UserId = userId });
            return roles.AsList().ToArray();
        }

        // ======================
        // RefreshTokens
        // ======================
        // Adds a new refresh token for a user
        public async Task AddRefreshTokenAsync(RefreshToken token)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                INSERT INTO RefreshTokens
                (RefreshTokenId, UserId, Token, ExpiresAt, CreatedAt, CreatedByIp)
                VALUES (@RefreshTokenId, @UserId, @Token, @ExpiresAt, SYSUTCDATETIME(), @CreatedByIp);
            ";
            await connection.ExecuteAsync(sql, token);
        }

        // Retrieves a refresh token by its token string if it is not revoked and not expired
        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT *
                FROM RefreshTokens
                WHERE Token = @Token
                  AND IsRevoked = 0
                  AND ExpiresAt > SYSUTCDATETIME();
            ";
            return await connection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }

        // Revokes a specific refresh token and optionally sets replaced token and revoked IP
        public async Task RevokeRefreshTokenAsync(string token, string replacedByToken = null, string revokedByIp = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE RefreshTokens
                SET RevokedAt = SYSUTCDATETIME(),
                    RevokedByIp = @RevokedByIp,
                    ReplacedByToken = @ReplacedByToken
                WHERE Token = @Token
                  AND RevokedAt IS NULL;
            ";
            await connection.ExecuteAsync(sql, new { Token = token, ReplacedByToken = replacedByToken, RevokedByIp = revokedByIp });
        }

        // Revokes all refresh tokens for a specific user
        public async Task RevokeAllRefreshTokensForUserAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE RefreshTokens
                SET RevokedAt = SYSUTCDATETIME(),
                    IsRevoked = 1
                WHERE UserId = @UserId
                  AND RevokedAt IS NULL;
            ";
            await connection.ExecuteAsync(sql, new { UserId = userId });
        }
    }
}
