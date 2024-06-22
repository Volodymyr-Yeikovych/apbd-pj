using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s28201_Project.Dto;
using s28201_Project.Service;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/contracts/individual/")]
public class IndividualContractController(IndividualContractService service) : ControllerBase
{
    [HttpGet("{id:long}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetContractAsync(long id)
    {
        var client = await service.GetContractByIdAsync(id);
        
        if (client == null) return NotFound($"Contract with id[{id}] was not found.");

        return Ok(client);
    }
    
    [HttpPost]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> AddContractAsync(ContractDto dto)
    {
        var response = await service.AddContractAsync(dto);

        if (!response.IsSuccessful) return BadRequest(response.Message);
        
        return NoContent();
    }
    
    [HttpPut("{id:long}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> SignContractAsync(long id)
    {
        var isSigned = await service.TrySignContractAsync(id);
    
        if (!isSigned) return BadRequest($"Failed to sign contract with id[{id}].");
         
        return NoContent();
    }
}