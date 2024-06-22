using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s28201_Project.Dto;
using s28201_Project.Service;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/clients/individual/")]
public class IndividualClientController(IndividualClientService service) : ControllerBase
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
    public async Task<IActionResult> AddClientAsync(IndividualClientDto dto)
    {
        var client = await service.AddClientAsync(dto);
        
        if (client == false) return BadRequest($"Unable to add client with pesel[{dto.Pesel}].");
        
        return NoContent();
    }
    
    [HttpPut]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateClientAsync(IndividualClientDto dto)
    {
        var result = await service.UpdateClientAsync(dto);

        if (result == false) return BadRequest($"Unable to update client with pesel[{dto.Pesel}].");
        
        return NoContent();
    }

    
    [HttpDelete("{pesel}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteClientAsync(string pesel)
    {
        var result = await service.DeleteClientAsync(pesel);

        if (result == false) return BadRequest($"Unable to delete client with pesel[{pesel}].");
        
        return NoContent();
    }
}