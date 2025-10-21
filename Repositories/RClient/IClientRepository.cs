using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories.RClient
{
    public interface IClientRepository
    {
        Task<List<ClientSummaryDto>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(Guid id);
        Task<Client> CreateClientAsync(Client client);
        Task<bool> UpdateClientAsync(Guid id, Client client);
        Task<(bool Success, int RowsAffected)> DeleteClientAsync(Guid id);
    }
}