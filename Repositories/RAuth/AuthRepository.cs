using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories
{
    public class AuthRepository(BookBorrowingContext dbContext) : IAuthRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<Client?> GetClientByUsernameAsync(string username)
        {
            return await dbContext.Clients.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            return await dbContext.Clients.FindAsync(id);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await dbContext.Clients.AnyAsync(u => u.Username == username);
        }

        public async Task AddClientAsync(Client client)
        {
            await dbContext.Clients.AddAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            dbContext.Clients.Update(client);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}