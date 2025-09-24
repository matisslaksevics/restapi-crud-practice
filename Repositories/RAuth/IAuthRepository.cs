using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories
{
    public interface IAuthRepository
    {
        Task<Client?> GetClientByUsernameAsync(string username);
        Task<Client?> GetClientByIdAsync(Guid id);
        Task<bool> UsernameExistsAsync(string username);
        Task<Client?> CreateClientAsync(Client client);
        Task<bool> UpdateClientAsync(Client client);
        Task<string?> GetHashedPasswordAsync(Guid userId);
    }
}