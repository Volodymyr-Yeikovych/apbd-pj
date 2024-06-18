using Microsoft.AspNetCore.Mvc;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/clients")]
public class ClientController : ControllerBase
{
    // TODO: Not Implemented
    [HttpGet("{id:long}")]
    public IActionResult GetClient(long id)
    {
        return Ok();
    }

    // TODO: Not Implemented
    [HttpPost]
    public IActionResult AddClient()
    {
        return NoContent();
    }

    // TODO: Not Implemented
    [HttpPut("{id:long}")]
    public IActionResult UpdateClient(long id)
    {
        return NoContent();
    }

    // TODO: Not Implemented
    [HttpDelete("{id:long}")]
    public IActionResult DeleteClient(long id)
    {
        return NoContent();
    }
}