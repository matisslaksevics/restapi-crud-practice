using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services
{
    public interface IClientService
    {
        Task<List<Client>> GetAllClientsAsync(BookBorrowingContext dbContext);
    }
}