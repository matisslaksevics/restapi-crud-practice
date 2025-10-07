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
            logger.LogInformation("GetClientByUsernameAsync requested.");
            try
            {
                var result = await dbContext.Clients.FirstOrDefaultAsync(u => u.Username == username);

                if (result is not null)
                {
                    logger.LogInformation("GetClientByUsernameAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("GetClientByUsernameAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GetClientByUsernameAsync failed.");
                throw;
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            logger.LogInformation("GetClientByIdAsync requested.");
            try
            {
                var result = await dbContext.Clients.FindAsync(id);

                if (result is not null)
                {
                    logger.LogInformation("GetClientByIdAsync successful.");
                    return result;
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

        public async Task<bool> UsernameExistsAsync(string username)
        {
            logger.LogInformation("UsernameExistsAsync requested.");
            try
            {
                var result = await dbContext.Clients.AnyAsync(u => u.Username == username);

                if (result)
                {
                    logger.LogInformation("UsernameExistsAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("UsernameExistsAsync failed.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "UsernameExistsAsync failed.");
                throw;
            }
        }

        public async Task<Client?> CreateClientAsync(Client client)
        {
            logger.LogInformation("CreateClientAsync requested.");
            try
            {
                var user = await dbContext.Clients.AddAsync(client);
                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    logger.LogInformation("CreateClientAsync successful.");
                    return user.Entity;
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

        public async Task<bool> UpdateClientAsync(Client client)
        {
            logger.LogInformation("UpdateClientAsync requested.");
            try
            {
                dbContext.Clients.Update(client);
                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    logger.LogInformation("UpdateClientAsync successful.");
                    return true;
                }
                else
                {
                    logger.LogError("UpdateClientAsync failed.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "UpdateClientAsync failed.");
                throw;
            }
        }
        public async Task<string?> GetHashedPasswordAsync(Guid userId)
        {
            logger.LogInformation("GetHashedPasswordAsync requested.");
            try
            {
                var user = await dbContext.Clients
                    .Where(c => c.Id == userId)
                    .Select(c => c.PasswordHash)
                    .FirstOrDefaultAsync();

                if (user is not null)
                {
                    logger.LogInformation("GetHashedPasswordAsync successful.");
                    return user;
                }
                else
                {
                    logger.LogError("GetHashedPasswordAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GetHashedPasswordAsync failed.");
                throw;
            }
        }
    }
}