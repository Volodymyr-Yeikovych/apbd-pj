using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s28201_Project.Dto;
using s28201_Project.Service.Installment;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/payments/individual/")]
public class IndividualInstallmentController(IndividualInstallmentService service) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> AddInstallmentAsync(IndividualInstallmentDto dto)
    {
        var response = await service.ProcessInstallmentAsync(dto);

        if (!response.IsSuccessful) return BadRequest(response.Message);
        
        return NoContent();
    }
}