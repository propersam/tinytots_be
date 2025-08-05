using Microsoft.AspNetCore.Mvc;

namespace Tinytots.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TinytotsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }

}