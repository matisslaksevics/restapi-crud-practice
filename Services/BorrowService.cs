using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;
namespace restapi_crud_practice.Services
{
    public class BorrowService
    {
        public async Task<List<Borrow>> GetAllBorrowsAsync(BookBorrowingContext dbContext)
        {
            return await dbContext.Borrows.ToListAsync();
        }


    }
}
