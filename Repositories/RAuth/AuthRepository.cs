using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories
{
    public class AuthRepository(BookBorrowingContext dbContext, ILogger<AuthRepository> logger) : IAuthRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<Client?> GetClientByUsernameAsync(string username)
        {
            logger.LogInformation("Repository: GetClientByUsernameAsync requested.");
            try
            {
                var result = await dbContext.Clients.FirstOrDefaultAsync(u => u.Username == username);

                if (result is not null)
                {
                    logger.LogInformation("Repository: GetClientByUsernameAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Repository: GetClientByUsernameAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetClientByUsernameAsync failed.");
                throw;
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            logger.LogInformation("Repository: GetClientByIdAsync requested.");
            try
            {
                var result = await dbContext.Clients.FindAsync(id);

                if (result is not null)
                {
                    logger.LogInformation("Repository: GetClientByIdAsync successful.");
                    return result;
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

        public async Task<bool> UsernameExistsAsync(string username)
        {
            logger.LogInformation("Repository: UsernameExistsAsync requested.");
            try
            {
                var result = await dbContext.Clients.AnyAsync(u => u.Username == username);

                if (result)
                {
                    logger.LogInformation("Repository: UsernameExistsAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Repository: UsernameExistsAsync failed.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: UsernameExistsAsync failed.");
                throw;
            }
        }

        public async Task<Client?> CreateClientAsync(Client client)
        {
            logger.LogInformation("Repository: CreateClientAsync requested.");
            try
            {
                var user = await dbContext.Clients.AddAsync(client);
                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    logger.LogInformation("Repository: CreateClientAsync successful.");
                    return user.Entity;
                }
                else
                {
                    logger.LogError("Repository: CreateClientAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: CreateClientAsync failed.");
                throw;
            }
        }

        public async Task<bool> UpdateClientAsync(Client client)
        {
            logger.LogInformation("Repository: UpdateClientAsync requested.");
            try
            {
                dbContext.Clients.Update(client);
                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    logger.LogInformation("Repository: UpdateClientAsync successful.");
                    return true;
                }
                else
                {
                    logger.LogError("Repository: UpdateClientAsync failed.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: UpdateClientAsync failed.");
                throw;
            }
        }
        public async Task<string?> GetHashedPasswordAsync(Guid userId)
        {
            logger.LogInformation("Repository: GetHashedPasswordAsync requested.");
            try
            {
                var user = await dbContext.Clients
                    .Where(c => c.Id == userId)
                    .Select(c => c.PasswordHash)
                    .FirstOrDefaultAsync();

                if (user is not null)
                {
                    logger.LogInformation("Repository: GetHashedPasswordAsync successful.");
                    return user;
                }
                else
                {
                    logger.LogError("Repository: GetHashedPasswordAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetHashedPasswordAsync failed.");
                throw;
            }
        }
    }
}