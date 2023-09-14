using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApi_MySQL.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DemoController : ControllerBase 
    {
        public DemoController()
        {
        }

        [HttpGet("policy/admin")]
        [Authorize(Policy = "Admin")]
        public IActionResult GetPolicyAdmin()
        {
            return Ok("Admin");
        }

        [HttpGet("policy/user")]
        [Authorize(Policy = "User")]
        public IActionResult GetPolicyUser()
        {
            return Ok("User");
        }

        [HttpGet("roles/admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRolesAdmin()
        {
            return Ok("Admin");
        }

        [HttpGet("roles/user")]
        [Authorize(Roles = "User")]
        public IActionResult GetRolesUser()
        {
            return Ok("User");
        }
    }
}
