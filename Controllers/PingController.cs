using Microsoft.AspNetCore.Mvc;

namespace MottothTracking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { pong = true, when = DateTime.UtcNow });
}