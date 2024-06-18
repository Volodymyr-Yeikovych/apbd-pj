using s28201_Project.Model;

namespace s28201_Project.Service;

public interface IClientService
{
    public Task<bool> AddClientAsync(Client client);
    public Task<bool> UpdateClientAsync(Client client);
    public Task<bool> DeleteClientAsync(Client client);
}