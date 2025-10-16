using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SMyLogger;

namespace restapi_crud_practice.Repositories.RClient
{
    public class ClientRepository(BookBorrowingContext dbContext, IMyLoggerService logger) : IClientRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<List<ClientSummaryDto>> GetAllClientsAsync()
        {
            logger.LogInfo("Repository: GetAllClients requested.");
            try
            {
                var clients = await dbContext.Clients
                .Select(client => client.ToClientSummaryDto())
                .ToListAsync();
                logger.LogInfo($"Repository: GetAllClients successful. Retrieved {clients.Count} clients.");
                return clients;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: GetAllClients failed.");
                throw;
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            logger.LogInfo("Repository: GetClientById requested.");
            try
            {
                var client = await dbContext.Clients.FindAsync(id);
                logger.LogInfo("Repository: GetClientById successful.");
                return client;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: GetClientById failed.");
                throw;
            }
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            logger.LogInfo("Repository: CreateClient requested.");
            try
            {
                dbContext.Clients.Add(client);
                await dbContext.SaveChangesAsync();
                logger.LogInfo("Repository: CreateClient successful.");
                return client;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: CreateClient failed.");
                throw;
            }
        }

        public async Task<bool> UpdateClientAsync(Guid id, Client client)
        {
            logger.LogInfo("Repository: UpdateClient requested.");
            try
            {
                var existingClient = await dbContext.Clients.FindAsync(id);
                if (existingClient is null)
                {
                    logger.LogError("Repository: UpdateClient failed.");
                    return false;
                }

                dbContext.Entry(existingClient).CurrentValues.SetValues(client);
                await dbContext.SaveChangesAsync();
                logger.LogInfo("Repository: UpdateClient successful.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: UpdateClient failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteClientAsync(Guid id)
        {
            logger.LogInfo("Repository: DeleteClient requested.");
            try
            {
                var result = await DbOperationHelper.ExecuteDeleteWithCountAsync(dbContext.Clients, client => client.Id == id);
                logger.LogInfo("Repository: DeleteClient successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: DeleteClient failed.");
                throw;
            }
        }
    }
}