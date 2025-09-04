using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Data
{
    public interface IClientRepository
    {
        Task<List<ClientSummaryDto>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(Guid id);
        Task<Client?> GetClientByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
        Task<Client> CreateClientAsync(Client client);
        Task<bool> UpdateClientAsync(Guid id, Client client);
        Task<bool> DeleteClientAsync(Guid id);
    }
}