using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeAutomation.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OfficeAutomation.API.Controllers
{
    // ✅ Requires the user to be authenticated for all actions in this controller
    [Authorize]

    // ✅ Sets the base route for this controller: /api/Cartable
    [Route("api/[controller]")]

    // ✅ Marks this class as an API controller
    [ApiController]
    public class CartableController : ControllerBase
    {
        private readonly ICartableService _service;

        // ==========================
        // Constructor
        // ==========================
        // Injects the cartable service
        public CartableController(ICartableService service) => _service = service;

        // ==========================
        // Helper property
        // ==========================
        // Retrieves the current authenticated user's ID from JWT claims
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ==========================
        // GET: /api/Cartable
        // ==========================
        // Retrieves all cartable items for the current user
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _service.GetMyCartableAsync(CurrentUserId);

            // Return list of cartable items
            return Ok(items);
        }
    }
}
