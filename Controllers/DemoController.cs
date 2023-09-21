using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApi_MySQL.Controllers
{
    //[ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DemoController : ControllerBase
    {
        private readonly ILogger _logger;

        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("policy/admin")]
        [Authorize(Policy = "Admin")]
        public IActionResult GetPolicyAdmin()
        {
            try
            {
                _logger.LogInformation("Policy Admin success");
                return Ok(new
                {
                    message = "Policy Admin"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Policy Admin error");
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("policy/user")]
        [Authorize(Policy = "User")]
        public IActionResult GetPolicyUser()
        {
            try
            {
                _logger.LogInformation("Policy User success");
                return Ok(new
                {
                    message = "Policy User"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Policy User error");
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("roles/admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRolesAdmin()
        {
            try
            {
                _logger.LogInformation("Roles Admin success");
                return Ok(new
                {
                    message = "Roles Admin"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Roles Admin error");
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("roles/user")]
        [Authorize(Roles = "User")]
        public IActionResult GetRolesUser()
        {
            try
            {
                _logger.LogInformation("Roles User success");
                return Ok(new
                {
                    message = "Roles User"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Roles User error");
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
