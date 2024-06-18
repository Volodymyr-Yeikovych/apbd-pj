using Microsoft.AspNetCore.Mvc;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/contracts")]
public class ContractController : ControllerBase
{
    // TODO: Not Implemented
    [HttpPost]
    public IActionResult AddContract()
    {
        return NoContent();
    }
    
    // TODO: Not Implemented
    [HttpPut]
    public IActionResult SignContract()
    {
        return NoContent();
    }
}