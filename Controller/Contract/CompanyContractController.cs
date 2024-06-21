using Microsoft.AspNetCore.Mvc;
using s28201_Project.Dto;
using s28201_Project.Service;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/contracts/company/")]
public class CompanyContractController(CompanyContractService service) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetContractAsync(long id)
    {
        var client = await service.GetContractByIdAsync(id);
        
        if (client == null) return NotFound($"Contract with id[{id}] was not found.");

        return Ok(client);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddContractAsync(ContractDto dto)
    {
        var response = await service.AddContractAsync(dto);

        if (!response.IsSuccessful) return BadRequest(response.Message);
        
        return NoContent();
    }
    
    // TODO: Not Implemented
    // [HttpPut("{id:long}")]
    // public async Task<IActionResult> SignContractAsync(long id)
    // {
    //     var isSigned = await service.SignContractAsync(id);
    //
    //     if (!isSigned) return BadRequest($"Contract with id[{id}] cannot be signed.");
    //      
    //     return NoContent();
    // }
    
}