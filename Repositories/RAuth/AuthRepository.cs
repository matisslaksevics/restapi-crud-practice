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

        public async Task<Client?> CreateClientAsync(Client client)
        {
            try
            {
                var result = await dbContext.Clients.AddAsync(client);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        public async Task<bool> UpdateClientAsync(Client client)
        {
            try
            {
                dbContext.Clients.Update(client);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<string?> GetHashedPasswordAsync(Guid userId)
        {
            var user = await dbContext.Clients
                .Where(c => c.Id == userId)
                .Select(c => c.PasswordHash) 
                .FirstOrDefaultAsync();

            return user;
        }
    }
}