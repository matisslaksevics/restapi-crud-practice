using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Data;
using restapi_crud_practice.Helpers;

namespace restapi_crud_practice.Repositories.RClient
{
    public class ClientRepository(BookBorrowingContext dbContext) : IClientRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<List<ClientSummaryDto>> GetAllClientsAsync()
        {
            return await dbContext.Clients
                .Select(client => client.ToClientSummaryDto())
                .ToListAsync();
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            return await dbContext.Clients.FindAsync(id);
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();
            return client;
        }

        public async Task<bool> UpdateClientAsync(Guid id, Client client)
        {
            var existingClient = await dbContext.Clients.FindAsync(id);
            if (existingClient is null)
            {
                return false;
            }

            dbContext.Entry(existingClient).CurrentValues.SetValues(client);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteClientAsync(Guid id)
        {
            return await DbOperationHelper.ExecuteDeleteAsync(dbContext.Clients, client => client.Id == id);
        }
    }
}