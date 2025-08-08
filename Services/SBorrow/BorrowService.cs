using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;

namespace restapi_crud_practice.Services.SBorrow
{
    public class BorrowService : IBorrowService
    {
        private readonly BookBorrowingContext dbContext;
        public BorrowService(BookBorrowingContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<BorrowSummaryDto>> GetAllBorrowsAsync()
        {
            return await dbContext.Borrows.Where(borrow => borrow.IsOverdue == true).Include(borrow => borrow.Client).Include(borrow => borrow.Book).Select(borrow => borrow.ToBorrowSummaryDto()).ToListAsync(); ;
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int id)
        {
            Borrow? borrow = await dbContext.Borrows.FindAsync(id);
            return borrow;
        }
        public async Task<BorrowSummaryDto> CreateBorrowAsync(CreateBorrowDto newBorrow)
        { 
            var borrow = newBorrow.ToEntity();
            borrow.Client = dbContext.Clients.Find(newBorrow.ClientId);
            borrow.Book = dbContext.Books.Find(newBorrow.BookId);
            if (borrow.ReturnDate is not null)
            {
                if (borrow.BorrowDate.AddMonths(3) < borrow.ReturnDate)
                {
                    borrow.IsOverdue = true;
                }
                else
                {
                    borrow.IsOverdue = false;
                }
            }
            dbContext.Borrows.Add(borrow);
            await dbContext.SaveChangesAsync();
            return borrow.ToBorrowSummaryDto();
        }
        public async Task<bool> UpdateBorrowAsync(int id, UpdateBorrowDto updatedBorrow)
        {
            var existingBorrow = await dbContext.Borrows.FindAsync(id);
            if (existingBorrow is null) return false;
            var modifiedBorrow = updatedBorrow with
            {
                IsOverdue = updatedBorrow.ReturnDate is not null && updatedBorrow.BorrowDate.AddMonths(3) < updatedBorrow.ReturnDate
            };
            dbContext.Entry(existingBorrow).CurrentValues.SetValues(modifiedBorrow.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<int> DeleteBorrowAsync(int id)
        {
            var response = await dbContext.Borrows.Where(borrow => borrow.Id == id).ExecuteDeleteAsync();
            return response;
        }
    }
}
