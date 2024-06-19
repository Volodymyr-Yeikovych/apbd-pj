using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using s28201_Project.Context;
using s28201_Project.Dto;
using s28201_Project.Model;

namespace s28201_Project.Service;

public class CompanyClientService(ApiContext context) : IClientService<CompanyClient, CompanyClientDto>
{
    public async Task<bool> AddClientAsync(CompanyClientDto dto)
    {
        var byKrs = await GetClientByKrsAsync(dto.KrsNum);
        if (byKrs != null) return false;
        
        var client = new CompanyClient(dto);
        
        await context.CompanyClients.AddAsync(client);
        await context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<bool> UpdateClientAsync(CompanyClientDto dto)
    {
        var byKrs = await GetClientByKrsAsync(dto.KrsNum);
        if (byKrs == null) return false;

        byKrs.Set(dto);
        
        await context.SaveChangesAsync();
        
        return true;
    }

    public async Task<CompanyClient?> GetClientByIdAsync(long id)
    {
        return await context.CompanyClients.FirstOrDefaultAsync(c => c.ClientId == id);
    }

    public async Task<CompanyClient?> GetClientByKrsAsync(string krs)
    {
        return await context.CompanyClients.FirstOrDefaultAsync(c => c.KrsNum == krs);
    }

    public async Task<decimal> GetMaximalDiscountAsync(CompanyClient client)
    {
        decimal max = client.Discounts.IsNullOrEmpty() ? 0 : await context.Discounts.Select(d => d.ValuePercent).MaxAsync();
        return max;
    }
}