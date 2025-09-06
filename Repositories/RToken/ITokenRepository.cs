using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories
{
    public interface ITokenRepository
    {
        Task<Client?> GetClientByIdAsync(Guid id);
    }
}
