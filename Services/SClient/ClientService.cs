using restapi_crud_practice.Repositories.RClient;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;

namespace restapi_crud_practice.Services.SClient
{
    public class ClientService(IClientRepository clientRepository, ILogger<ClientService> logger) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<List<ClientSummaryDto>> GetAllClientsAsync()
        {
            logger.LogInformation("Service: GetAllClientsAsync requested.");
            try
            {
                var result = await _clientRepository.GetAllClientsAsync();
                logger.LogInformation("Service: GetAllClientsAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GetAllClientsAsync failed.");
                throw;
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            logger.LogInformation("Service: GetClientByIdAsync requested.");
            try
            {
                var result = await _clientRepository.GetClientByIdAsync(id);

                if (result is not null)
                {
                    logger.LogInformation("Service: GetClientByIdAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Service: GetClientByIdAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GetClientByIdAsync failed.");
                throw;
            }
        }

        public async Task<ClientSummaryDto> CreateClientAsync(CreateClientDto newClient)
        {
            logger.LogInformation("Service: CreateClientAsync requested.");
            try
            {
                var client = newClient.ToEntity();
                var createdClient = await _clientRepository.CreateClientAsync(client);
                logger.LogInformation("Service: CreateClientAsync successful.");
                return createdClient.ToClientSummaryDto();

            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: CreateClientAsync failed.");
                throw;
            }
        }

        public async Task<bool> UpdateClientAsync(Guid id, UpdateClientDto updatedClient)
        {
            logger.LogInformation("Service: UpdateClientAsync requested.");
            try
            {
                var clientEntity = updatedClient.ToEntity(id);
                var result = await _clientRepository.UpdateClientAsync(id, clientEntity);

                if (result)
                {
                    logger.LogInformation("Service: UpdateClientAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Service: UpdateClientAsync failed.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: UpdateClientAsync failed.");
                throw;
            }
        }

        public async Task<(bool Success, int RowsAffected)> DeleteClientAsync(Guid id)
        {
            logger.LogInformation("Service: DeleteClientAsync requested.");
            try
            {
                var result = await _clientRepository.DeleteClientAsync(id);

                if (result is not (false, 0))
                {
                    logger.LogInformation("Service: DeleteClientAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Service: DeleteClientAsync failed.");
                    return (false, 0);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: DeleteClientAsync failed.");
                throw;
            }
        }
    }
}