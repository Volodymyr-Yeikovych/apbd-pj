using Microsoft.AspNetCore.Mvc;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/revenue")]
public class RevenueController : ControllerBase
{
    // TODO: Not Implemented
    [HttpGet("/all")]
    public IActionResult GetRevenueCompany()
    {
        return Ok();
    }
    
    // TODO: Not Implemented
    [HttpGet("/all-predicted")]
    public IActionResult GetPredictedRevenueCompany()
    {
        return Ok();
    }
    
    // TODO: Not Implemented
    [HttpGet("/product/{id:long}")]
    public IActionResult GetRevenueProduct(long id)
    {
        return Ok();
    }
    
    // TODO: Not Implemented
    [HttpGet("/product-predicted/{id:long}")]
    public IActionResult GetPredictedRevenueProduct(long id)
    {
        return Ok();
    }
}