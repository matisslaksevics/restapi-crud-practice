using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories.RToken
{
    public class TokenRepository(BookBorrowingContext dbContext) : ITokenRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            return await dbContext.Clients.FindAsync(id);
        }
    }
}