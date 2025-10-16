using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Repositories.RClient;
using restapi_crud_practice.Services.SMyLogger;
using System.Net;

namespace restapi_crud_practice.Services.SClient
{
    public class ClientService(IClientRepository clientRepository, IMyLoggerService logger) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<List<ClientSummaryDto>> GetAllClientsAsync()
        {
            logger.LogInfo("Service: GetAllClients requested.");
            try
            {
                var clients = await _clientRepository.GetAllClientsAsync();
                logger.LogInfo($"Service: GetAllClients successful. Retrieved {clients.Count} books.");
                return clients;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: GetAllClients failed.");
                throw;
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            logger.LogInfo("Service: GetClientById requested.");
            try
            {
                var client = await _clientRepository.GetClientByIdAsync(id);
                logger.LogInfo($"Service: GetClientById successful.");
                return client;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: GetClientById failed.");
                throw;
            }
        }

        public async Task<ClientSummaryDto> CreateClientAsync(CreateClientDto newClient)
        {
            logger.LogInfo("Service: CreateClient requested.");
            try
            {
                var client = newClient.ToEntity();
                var createdClient = await _clientRepository.CreateClientAsync(client);
                logger.LogInfo($"Service: CreateClient successful.");
                return createdClient.ToClientSummaryDto();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: CreateClient failed.");
                throw;
            }
        }

        public async Task<bool> UpdateClientAsync(Guid id, UpdateClientDto updatedClient)
        {
            logger.LogInfo("Service: UpdateClient requested.");
            try
            {
                var clientEntity = updatedClient.ToEntity(id);
                var result = await _clientRepository.UpdateClientAsync(id, clientEntity);
                logger.LogInfo($"Service: UpdateClient successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: UpdateClient failed.");
                throw;
            }
        }

        public async Task<(bool Success, int RowsAffected)> DeleteClientAsync(Guid id)
        {
            logger.LogInfo("Service: DeleteClient requested.");
            try
            {
                var result = await _clientRepository.DeleteClientAsync(id);
                logger.LogInfo($"Service: DeleteClient successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: DeleteClient failed.");
                throw;
            }
        }
    }
}