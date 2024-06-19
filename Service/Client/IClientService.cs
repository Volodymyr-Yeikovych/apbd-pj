using s28201_Project.Dto;
using s28201_Project.Model;

namespace s28201_Project.Service;

public interface IClientService<TClient, TClientDto> 
    where TClient : Client
    where TClientDto : ClientDto
{
    public Task<bool> AddClientAsync(TClientDto dto);
    public Task<bool> UpdateClientAsync(TClientDto dto);
    public Task<TClient?> GetClientByIdAsync(long id);
}