using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Mapping;

namespace restapi_crud_practice.Repositories.RClient
{
    public class ClientRepository(BookBorrowingContext dbContext, ILogger<ClientRepository> logger) : IClientRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<List<ClientSummaryDto>> GetAllClientsAsync()
        {
            logger.LogInformation("Repository: GetAllClientsAsync requested.");
            try
            { 
                var result = await dbContext.Clients
                .Select(client => client.ToClientSummaryDto())
                .ToListAsync();

                logger.LogInformation("Repository: GetAllClientsAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetAllClientsAsync failed.");
                throw;
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            var client = await dbContext.Clients.FindAsync(id);
            logger.LogInformation("Repository: GetClientByIdAsync requested.");
            try
            {

                var result = await dbContext.Clients
                .Select(client => client.ToClientSummaryDto())
                .ToListAsync();

                if (result is not null)
                {
                    logger.LogInformation("Repository: GetClientByIdAsync successful.");
                    return client;
                }
                else
                {
                    logger.LogError("Repository: GetClientByIdAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetClientByIdAsync failed.");
                throw;
            }
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            logger.LogInformation("Repository: CreateClientAsync requested.");
            try
            {
                dbContext.Clients.Add(client);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Repository: CreateClientAsync successful.");
                return client;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: CreateClientAsync failed.");
                throw;
            }
        }

        public async Task<bool> UpdateClientAsync(Guid id, Client client)
        {
            logger.LogInformation("Repository: UpdateClientAsync requested.");
            try
            {
                var existingClient = await dbContext.Clients.FindAsync(id);
                if (existingClient is null)
                {
                    logger.LogError("Repository: UpdateClientAsync failed.");
                    return false;
                }

                dbContext.Entry(existingClient).CurrentValues.SetValues(client);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Repository: UpdateClientAsync successful.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: UpdateClientAsync failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteClientAsync(Guid id)
        {
            logger.LogInformation("Repository: DeleteClientAsync requested.");
            try
            {
                var result = await DbOperationHelper.ExecuteDeleteWithCountAsync(dbContext.Clients, client => client.Id == id);
                if (result is (false, 0))
                {
                    logger.LogError("Repository: DeleteClientAsync failed.");
                    return (false, 0);
                }

                logger.LogInformation("Repository: DeleteClientAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: UpdateClientAsync failed.");
                throw;
            }
        }
    }
}