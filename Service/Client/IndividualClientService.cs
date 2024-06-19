using Microsoft.EntityFrameworkCore;
using s28201_Project.Context;
using s28201_Project.Dto;
using s28201_Project.Model;

namespace s28201_Project.Service;

public class IndividualClientService(ApiContext context) : IClientService<IndividualClient, IndividualClientDto>
{
    public async Task<bool> AddClientAsync(IndividualClientDto dto)
    {
        var byPesel = await GetClientByPeselAsync(dto.Pesel);
        if (byPesel != null) return false;
        
        var client = new IndividualClient(dto);
        
        await context.IndividualClients.AddAsync(client);
        await context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<bool> UpdateClientAsync(IndividualClientDto dto)
    {
        var byPesel = await GetClientByPeselAsync(dto.Pesel);
        if (byPesel == null) return false;

        byPesel.Set(dto);
        
        await context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteClientAsync(string pesel)
    {
        var byPesel = await GetClientByPeselAsync(pesel);
        if (byPesel == null) return false;

        byPesel.IsDeleted = true;
        await context.SaveChangesAsync();
        
        return true;
    }

    public async Task<IndividualClient?> GetClientByIdAsync(long id)
    {
        return await context.IndividualClients.FirstOrDefaultAsync(c => c.ClientId == id);
    }

    public async Task<IndividualClient?> GetClientByPeselAsync(string pesel)
    {
        return await context.IndividualClients.FirstOrDefaultAsync(c => c.Pesel == pesel);
    }
}