using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;

namespace restapi_crud_practice.Data
{
    public class ClientRepository : IClientRepository
    {
        private readonly BookBorrowingContext dbContext;
        public ClientRepository(BookBorrowingContext dbContext)
        {
            this.dbContext = dbContext;
        }

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

        public async Task<Client?> GetClientByUsernameAsync(string username)
        {
            return await dbContext.Clients
                .FirstOrDefaultAsync(c => c.Username == username);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await dbContext.Clients
                .AnyAsync(c => c.Username == username);
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
            if (existingClient is null) return false;

            dbContext.Entry(existingClient).CurrentValues.SetValues(client);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteClientAsync(Guid id)
        {
            var client = await dbContext.Clients.FindAsync(id);
            if (client is null) return false;

            dbContext.Clients.Remove(client);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}