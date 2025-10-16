using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Services.SMyLogger;
namespace restapi_crud_practice.Repositories.RBook
{
    public class BookRepository(BookBorrowingContext dbContext, IMyLoggerService logger) : IBookRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<List<Book>> GetAllBooksAsync()
        {
            logger.LogInfo("Repository: GetAllBooks requested.");
            try
            {
                var books = await dbContext.Books.ToListAsync();
                logger.LogInfo($"Repository: GetAllBooks successful. Retrieved {books.Count} books.");
            return books;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: GetAllBooks failed.");
                throw;
            }
        }                                                                                   

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            logger.LogInfo("Repository: GetBookById requested.");
            try
            {
                var book = await dbContext.Books.FindAsync(id);
                logger.LogInfo("Repository: GetBookById successful.");
                return book;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: GetBookById failed.");
                throw;
            }
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            logger.LogInfo("Repository: CreateBook requested.");
            try
            {
                dbContext.Books.Add(book);
                await dbContext.SaveChangesAsync();
                logger.LogInfo("Repository: CreateBook successful.");
                return book;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: CreateBook failed.");
                throw;
            }
        }
        public async Task<bool> UpdateBookAsync(int id, Book book)
        {
            logger.LogInfo("Repository: UpdateBook requested.");
            try
            {
                var existingBook = await dbContext.Books.FindAsync(id);
                if (existingBook is null)
                {
                    logger.LogError("Repository: UpdateBook failed.");
                    return false;
                }

                dbContext.Entry(existingBook).CurrentValues.SetValues(book);
                await dbContext.SaveChangesAsync();
                logger.LogInfo("Repository: UpdateBook successful.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: UpdateBook failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteBookAsync(int id)
        {
            logger.LogInfo("Repository: GetAllBooks requested.");
            try
            {
                var result = await DbOperationHelper.ExecuteDeleteWithCountAsync(dbContext.Books, book => book.Id == id);
                logger.LogInfo("Repository: DeleteBook successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Repository: DeleteBook failed.");
                throw;
            }
        }
    }
}
