using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
namespace restapi_crud_practice.Services.SClient
{
    public class ClientService : IClientService
    {
        private readonly BookBorrowingContext dbContext;
        public ClientService(BookBorrowingContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<ClientSummaryDto>> GetAllClientsAsync()
        {
            return await dbContext.Clients.Select(client => client.ToClientSummaryDto()).ToListAsync();
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            Client? client = await dbContext.Clients.FindAsync(id);
            return client;
        }
        public async Task<ClientSummaryDto> CreateClientAsync(CreateClientDto newClient)
        {
            var client = newClient.ToEntity();
            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();
            return client.ToClientSummaryDto();
        }
        public async Task<bool> UpdateClientAsync(Guid id, UpdateClientDto updatedClient)
        {
            var existingClient = await dbContext.Clients.FindAsync(id);
            if (existingClient is null) return false;
            dbContext.Entry(existingClient).CurrentValues.SetValues(updatedClient.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteClientAsync(Guid id)
        { 
            var cmd = await dbContext.Clients.Where(client => client.Id == id).ExecuteDeleteAsync();
            if (cmd != 0)
            {
                return true;
            }
            return false;
        }


    }
}
