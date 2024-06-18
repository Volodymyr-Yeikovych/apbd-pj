using Microsoft.AspNetCore.Mvc;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    // TODO: Not Implemented
    [HttpPost]
    public IActionResult AddPayment()
    {
        return NoContent();
    }
}