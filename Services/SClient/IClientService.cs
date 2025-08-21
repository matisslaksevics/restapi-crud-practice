using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services.SClient
{
    public interface IClientService
    {
        Task<List<ClientSummaryDto>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<ClientSummaryDto> CreateClientAsync(CreateClientDto newClient);
        Task<bool> UpdateClientAsync(int id, UpdateClientDto updatedClient);
        Task<bool> DeleteClientAsync(int id);
    }
}