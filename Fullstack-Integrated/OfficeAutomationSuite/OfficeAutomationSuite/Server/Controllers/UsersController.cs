using Microsoft.AspNetCore.Mvc;
using OfficeAutomationSuite.Shared.Models;
using OfficeAutomationSuite.Server.Services;

namespace OfficeAutomationSuite.Server.Controllers
{
    // Controller for user-related API endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // ---------------- Register endpoint ----------------
        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (!result)
                return BadRequest("Username already exists.");  // Username already taken

            return Ok("User registered successfully.");  // Registration successful
        }

        // ---------------- Login endpoint ----------------
        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _userService.LoginAsync(dto.Username, dto.Password);
            if (token == null)
                return Unauthorized("Invalid username or password.");  // Authentication failed

            // Return JWT token
            return Ok(new { Token = token });
        }
    }
}
