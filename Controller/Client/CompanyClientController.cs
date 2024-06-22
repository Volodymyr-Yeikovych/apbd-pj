using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s28201_Project.Dto;
using s28201_Project.Service;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/clients/company/")]
public class CompanyClientController(CompanyClientService service) : ControllerBase
{
    [HttpGet("{id:long}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetClientAsync(long id)
    {
        var client = await service.GetClientByIdAsync(id);
        
        if (client == null) return NotFound($"Individual Client with id[{id}] was not found.");

        return Ok(client);
    }
    
    [HttpPost]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> AddClientAsync(CompanyClientDto dto)
    {
        var client = await service.AddClientAsync(dto);
        
        if (client == false) return BadRequest($"Unable to add client with KRS[{dto.KrsNum}]");
        
        return NoContent();
    }
    
    [HttpPut]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateClientAsync(CompanyClientDto dto)
    {
        var result = await service.UpdateClientAsync(dto);

        if (result == false) return BadRequest($"Unable to update client with KRS[{dto.KrsNum}]");
        
        return NoContent();
    }
}