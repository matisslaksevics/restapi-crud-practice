using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Mapping;

namespace restapi_crud_practice.Repositories.RBorrow
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly BookBorrowingContext dbContext;
        public BorrowRepository(BookBorrowingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<BorrowSummaryDto>> GetAllBorrowsAsync()
        {
            return await dbContext.Borrows.Where(borrow => borrow.IsOverdue == true)
                .Include(borrow => borrow.Client)
                .Include(borrow => borrow.Book)
                .Select(borrow => borrow
                .ToBorrowSummaryDto())
                .ToListAsync();
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int id)
        {
            return await dbContext.Borrows
                .Include(b => b.Client)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<List<BorrowSummaryDto>> GetAllClientBorrowsAsync(Guid? userId)
        {
            return await dbContext.Borrows.Where(borrow => borrow.IsOverdue == true)
                .Where(borrow => borrow.ClientId == userId)
                .Include(borrow => borrow.Client)
                .Include(borrow => borrow.Book)
                .Select(borrow => borrow
                .ToBorrowSummaryDto())
                .ToListAsync();
        }

        public async Task<Borrow?> CreateBorrowAsync(Borrow borrow)
        {
            dbContext.Borrows.Add(borrow);
            await dbContext.SaveChangesAsync();
            return await dbContext.Borrows
                .Include(b => b.Client)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == borrow.Id);
        }
        public async Task<Client?> GetClientAsync(Guid? clientId)
        {
            if (clientId == null)
            {
                return null;
            } 

            return await dbContext.Clients.FindAsync(clientId);
        }

        public async Task<Book?> GetBookAsync(int bookId)
        {
            return await dbContext.Books.FindAsync(bookId);
        }

        public async Task<bool> UpdateBorrowAsync(int id, Borrow borrow)
        {
            var existingBorrow = await dbContext.Borrows.FindAsync(id);
            if (existingBorrow is null)
            {
                return false;
            }

            dbContext.Entry(existingBorrow).CurrentValues.SetValues(borrow);
            existingBorrow.IsOverdue = BorrowHelper.CalculateIsOverdue(borrow.BorrowDate, borrow.ReturnDate);
            existingBorrow.ClientId = borrow.ClientId;
            existingBorrow.BookId = borrow.BookId;

            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteBorrowAsync(int id)
        {
            return await DbOperationHelper.ExecuteDeleteAsync(dbContext.Borrows, borrow => borrow.Id == id);
        }
    }
}
