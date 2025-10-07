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
            logger.LogInformation("GetAllClientsAsync requested.");
            try
            {

                var result = await dbContext.Clients
                .Select(client => client.ToClientSummaryDto())
                .ToListAsync();

                if (result is not null)
                {
                    logger.LogInformation("GetAllClientsAsync successful.");
                    return result;
                } else 
                {
                    logger.LogError("GetAllClientsAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GetAllClientsAsync failed.");
                throw;
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            var client = await dbContext.Clients.FindAsync(id);
            logger.LogInformation("GetClientByIdAsync requested.");
            try
            {

                var result = await dbContext.Clients
                .Select(client => client.ToClientSummaryDto())
                .ToListAsync();

                if (result is not null)
                {
                    logger.LogInformation("GetClientByIdAsync successful.");
                    return client;
                }
                else
                {
                    logger.LogError("GetClientByIdAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GetClientByIdAsync failed.");
                throw;
            }
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            logger.LogInformation("CreateClientAsync requested.");
            try
            {
                dbContext.Clients.Add(client);
                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    logger.LogInformation("CreateClientAsync successful.");
                    return client;
                }
                else
                {
                    logger.LogError("CreateClientAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "CreateClientAsync failed.");
                throw;
            }
        }

        public async Task<bool> UpdateClientAsync(Guid id, Client client)
        {
            logger.LogInformation("UpdateClientAsync requested.");
            try
            {
                var existingClient = await dbContext.Clients.FindAsync(id);
                if (existingClient is null)
                {
                    logger.LogError("UpdateClientAsync failed.");
                    return false;
                }

                dbContext.Entry(existingClient).CurrentValues.SetValues(client);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("UpdateClientAsync successful.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "UpdateClientAsync failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteClientAsync(Guid id)
        {
            logger.LogInformation("DeleteClientAsync requested.");
            try
            {
                var result = await DbOperationHelper.ExecuteDeleteWithCountAsync(dbContext.Clients, client => client.Id == id);
                if (result is (false, 0))
                {
                    logger.LogError("DeleteClientAsync failed.");
                    return (false, 0);
                }

                logger.LogInformation("DeleteClientAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "UpdateClientAsync failed.");
                throw;
            }
        }
    }
}