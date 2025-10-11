using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;

namespace restapi_crud_practice.Repositories.RBook
{
    public class BookRepository(BookBorrowingContext dbContext, ILogger<BookRepository> logger) : IBookRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<List<Book>> GetAllBooksAsync()
        {
            logger.LogInformation("Repository: GetAllBooksAsync requested.");
            try
            {
                var result = await dbContext.Books.ToListAsync();
                logger.LogInformation("Repository: GetAllBooksAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetAllBooksAsync failed.");
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            logger.LogInformation("Repository: GetBookByIdAsync requested.");
            try
            {

                var result = await dbContext.Books.FindAsync(id);

                if (result is not null)
                {
                    logger.LogInformation("Repository: GetBookByIdAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Repository: GetBookByIdAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetBookByIdAsync failed.");
                throw;
            }
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            logger.LogInformation("Repository: CreateBookAsync requested.");
            try
            {
                dbContext.Books.Add(book);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Repository: CreateBookAsync successful.");
                return book;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: CreateBookAsync failed.");
                throw;
            }
        }
        public async Task<bool> UpdateBookAsync(int id, Book book)
        {
            logger.LogInformation("Repository: UpdateBookAsync requested.");
            try
            {
                var existingBook = await dbContext.Books.FindAsync(id);
                if (existingBook is null)
                {
                    logger.LogError("Repository: UpdateBookAsync failed.");
                    return false;
                }

                dbContext.Entry(existingBook).CurrentValues.SetValues(book);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Repository: UpdateBookAsync successful.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: UpdateBookAsync failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteBookAsync(int id)
        {
            logger.LogInformation("Repository: DeleteBookAsync requested.");
            try
            {
                var result = await DbOperationHelper.ExecuteDeleteWithCountAsync(dbContext.Books, book => book.Id == id);
                if (result is (false, 0))
                {
                    logger.LogError("Repository: DeleteBookAsync failed.");
                    return (false, 0);
                }

                logger.LogInformation("Repository: DeleteBookAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: DeleteBookAsync failed.");
                throw;
            }
        }
    }
}
