using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Mapping;
namespace restapi_crud_practice.Repositories.RBook
{
    public class BookRepository(BookBorrowingContext dbContext) : IBookRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<List<BookSummaryDto>> GetAllBooksAsync()
        {
            return await dbContext.Books
                .Select(book => book
                .ToBookSummaryDto()).ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await dbContext.Books.FindAsync(id);
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();
            return book;
        }
        public async Task<bool> UpdateBookAsync(int id, Book book)
        {
            var existingBook = await dbContext.Books.FindAsync(id);
            if (existingBook is null)
            {
                return false;
            }

            dbContext.Entry(existingBook).CurrentValues.SetValues(book);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteBookAsync(int id)
        {
            return await DbOperationHelper.ExecuteDeleteAsync(dbContext.Books, book => book.Id == id);
        }
    }
}
