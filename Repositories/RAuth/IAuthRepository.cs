using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories
{
    public interface IAuthRepository
    {
        Task<Client?> GetClientByUsernameAsync(string username);
        Task<Client?> GetClientByIdAsync(Guid id);
        Task<bool> UsernameExistsAsync(string username);
        Task AddClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task SaveChangesAsync();
    }
}