using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceStationV_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet("verify")]
        public IActionResult Verify()
        {
            return Ok(new { valid = true });
        }
    }
}
