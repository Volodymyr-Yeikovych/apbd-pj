using s28201_Project.Model;

namespace s28201_Project.Service;

public class ClientService
    : IClientService
{
    private static readonly Dictionary<ClientType, IClientService> ServiceMap = new ();

    private static void Init(Dictionary<ClientType, IClientService> clientServices)
    {
        foreach (var kvp in clientServices)
        {
            ServiceMap[kvp.Key] = kvp.Value;
        }
    }
    
    public ClientService(Dictionary<ClientType, IClientService> clientServices)
    {
        Init(clientServices);
    }

    public async Task<bool> AddClientAsync(Client client)
    {
        return client switch
        {
            IndividualClient => await ServiceMap[ClientType.Individual].AddClientAsync(client),
            CompanyClient => await ServiceMap[ClientType.Company].AddClientAsync(client),
            _ => false
        };
    }

    public async Task<bool> UpdateClientAsync(Client client)
    {
        return client switch
        {
            IndividualClient => await ServiceMap[ClientType.Individual].UpdateClientAsync(client),
            CompanyClient => await ServiceMap[ClientType.Company].UpdateClientAsync(client),
            _ => false
        };
    }

    public async Task<bool> DeleteClientAsync(Client client)
    {
        return client switch
        {
            IndividualClient => await ServiceMap[ClientType.Individual].DeleteClientAsync(client),
            CompanyClient => await ServiceMap[ClientType.Company].DeleteClientAsync(client),
            _ => false
        };
    }
}