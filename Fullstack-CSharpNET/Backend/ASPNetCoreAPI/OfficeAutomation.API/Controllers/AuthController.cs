using Microsoft.AspNetCore.Mvc;
using OfficeAutomation.Core.DTOs;
using OfficeAutomation.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OfficeAutomation.API.Controllers
{
    // ✅ Defines this class as an API controller
    [ApiController]

    // ✅ Sets the base route for this controller: /api/Auth
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // ==========================
        // Constructor
        // ==========================
        // Injects the authentication service
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ==========================
        // POST: /api/Auth/login
        // ==========================
        // Handles user login requests
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto login,          // User login credentials
            [FromQuery] string captchaToken,    // Captcha token for validation
            [FromQuery] string clientType,      // Client type (web, mobile, etc.)
            [FromQuery] string ipAddress,       // User IP address
            [FromQuery] string deviceInfo       // User device information
        )
        {
            try
            {
                // Call AuthService to perform login
                var result = await _authService.LoginAsync(
                    login.UserName,
                    login.Password,
                    captchaToken,
                    clientType,
                    ipAddress,
                    deviceInfo
                );

                // Return successful login response with tokens
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return error response if login fails
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
