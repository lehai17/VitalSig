using Microsoft.AspNetCore.Mvc;

namespace Vitalsig.API.Controllers;

[ApiController]
[Route("api/system")]
public class SystemController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            name = "Vitalsig API",
            status = "Healthy",
            utcNow = DateTime.UtcNow
        });
    }
}
