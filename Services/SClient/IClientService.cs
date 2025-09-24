using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services.SClient
{
    public interface IClientService
    {
        Task<List<ClientSummaryDto>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(Guid id);
        Task<ClientSummaryDto> CreateClientAsync(CreateClientDto newClient);
        Task<bool> UpdateClientAsync(Guid id, UpdateClientDto updatedClient);
        Task<(bool Success, int RowsAffected)> DeleteClientAsync(Guid id); 
    }
}