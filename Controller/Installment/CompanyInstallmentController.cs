using Microsoft.AspNetCore.Mvc;
using s28201_Project.Dto;
using s28201_Project.Service.Installment;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/payments/company")]
public class CompanyInstallmentController(CompanyInstallmentService service) : ControllerBase
{
    // TODO: Not Implemented
    [HttpPost]
    public async Task<IActionResult> AddInstallmentAsync(CompanyInstallmentDto dto)
    {
        var response = await service.ProcessInstallmentAsync(dto);

        if (!response.IsSuccessful) return BadRequest(response.message);
        
        return NoContent();
    }
}