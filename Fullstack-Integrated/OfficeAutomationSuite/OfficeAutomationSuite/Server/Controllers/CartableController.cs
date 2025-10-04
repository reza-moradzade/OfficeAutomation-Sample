using Microsoft.AspNetCore.Mvc;
using OfficeAutomationSuite.Server.Services;
using OfficeAutomationSuite.Shared.Models;

namespace OfficeAutomationSuite.Server.Controllers
{
    // Controller for cartable (task/assignment) API endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class CartableController : ControllerBase
    {
        private readonly CartableService _service;

        public CartableController(CartableService service)
        {
            _service = service;
        }

        // ---------------- Get all cartables ----------------
        // GET: api/cartable
        [HttpGet]
        public async Task<ActionResult<List<CartableDto>>> GetAll()
        {
            var cartables = await _service.GetAllCartablesAsync();
            return Ok(cartables);  // Return list of cartables
        }
    }
}
