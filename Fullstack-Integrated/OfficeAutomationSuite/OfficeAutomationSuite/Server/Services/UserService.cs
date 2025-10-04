using OfficeAutomationSuite.Shared.Models;
using OfficeAutomationSuite.Server.Data.Entities;
using OfficeAutomationSuite.Server.Repositories;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OfficeAutomationSuite.Server.Services
{
    // Service for handling user-related operations
    // Includes registration, login, and password hashing
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly IConfiguration _configuration;

        public UserService(UserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        // ---------------- Register a new user ----------------
        public async Task<bool> RegisterAsync(UserRegisterDto dto)
        {
            // Check if the username already exists
            var existingUser = await _repository.GetByUsernameAsync(dto.Username);
            if (existingUser != null) return false;

            // Use a fixed salt for testing
            var salt = "SALT-REZA-2025";

            // Compute SHA256 password hash as hex string
            var passwordHash = HashPassword(dto.Password, salt);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FullName = dto.FullName,
                Salt = salt,
                PasswordHash = passwordHash,
                IsActive = true,
                IsDeleted = false,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save user to database
            await _repository.AddAsync(user);
            return true;
        }

        // ---------------- Login and generate JWT ----------------
        public async Task<string> LoginAsync(string username, string password)
        {
            // Get user by username
            var user = await _repository.GetByUsernameAsync(username);
            if (user == null) return null;

            // Hash the input password using the stored salt
            var inputHash = HashPassword(password, user.Salt);
            if (inputHash != user.PasswordHash) return null;

            // Create JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // ---------------- Hash password using SHA256 ----------------
        private string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(bytes);

            // Convert hash bytes to hex string
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
