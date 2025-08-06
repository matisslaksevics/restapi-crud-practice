using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;
namespace restapi_crud_practice.Services
{
    public class ClientService
    {
        public async Task<List<Client>> GetAllClientsAsync(BookBorrowingContext dbContext)
        {
            return await dbContext.Clients.ToListAsync();
        }
    }
}
