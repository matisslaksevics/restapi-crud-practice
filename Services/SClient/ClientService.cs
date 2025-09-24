using restapi_crud_practice.Repositories.RClient;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;

namespace restapi_crud_practice.Services.SClient
{
    public class ClientService(IClientRepository clientRepository) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<List<ClientSummaryDto>> GetAllClientsAsync()
        {
            return await _clientRepository.GetAllClientsAsync();
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            return await _clientRepository.GetClientByIdAsync(id);
        }

        public async Task<ClientSummaryDto> CreateClientAsync(CreateClientDto newClient)
        {
            var client = newClient.ToEntity();
            var createdClient = await _clientRepository.CreateClientAsync(client);
            return createdClient.ToClientSummaryDto();
        }

        public async Task<bool> UpdateClientAsync(Guid id, UpdateClientDto updatedClient)
        {
            var clientEntity = updatedClient.ToEntity(id);
            return await _clientRepository.UpdateClientAsync(id, clientEntity);
        }

        public async Task<(bool Success, int RowsAffected)> DeleteClientAsync(Guid id)
        {
            return await _clientRepository.DeleteClientAsync(id);
        }
    }
}