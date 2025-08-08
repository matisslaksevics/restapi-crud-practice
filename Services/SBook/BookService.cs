using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
namespace restapi_crud_practice.Services.SBook
{
    public class BookService : IBookService
    {
        private readonly BookBorrowingContext dbContext;
        public BookService(BookBorrowingContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<BookSummaryDto>> GetAllBooksAsync()
        {
            return await dbContext.Books.Select(book => book.ToBookSummaryDto()).ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            Book? book = await dbContext.Books.FindAsync(id);
            return book;
        }
        public async Task<BookSummaryDto> CreateBookAsync(CreateBookDto newBook)
        {
            var book = newBook.ToEntity();
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();
            return book.ToBookSummaryDto();
        }
        public async Task<bool> UpdateBookAsync(int id, UpdateBookDto updatedBook)
        {
            var existingBook = await dbContext.Books.FindAsync(id);
            if (existingBook is null) return false;
            dbContext.Entry(existingBook).CurrentValues.SetValues(updatedBook.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<int> DeleteBookAsync(int id)
        {
            var response = await dbContext.Books.Where(book => book.Id == id).ExecuteDeleteAsync();
            return response;
        }
    }
}
