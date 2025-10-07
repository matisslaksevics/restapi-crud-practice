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
            logger.LogInformation("GetAllBooksAsync requested.");
            try
            {

                var result = await dbContext.Books.ToListAsync();

                if (result is not null)
                {
                    logger.LogInformation("GetAllBooksAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("GetAllBooksAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GetAllBooksAsync failed.");
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            logger.LogInformation("GetBookByIdAsync requested.");
            try
            {

                var result = await dbContext.Books.FindAsync(id);

                if (result is not null)
                {
                    logger.LogInformation("GetBookByIdAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("GetBookByIdAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GetBookByIdAsync failed.");
                throw;
            }
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            logger.LogInformation("CreateBookAsync requested.");
            try
            {
                dbContext.Books.Add(book);
                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    logger.LogInformation("CreateBookAsync successful.");
                    return book;
                }
                else
                {
                    logger.LogError("CreateBookAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "CreateBookAsync failed.");
                throw;
            }
        }
        public async Task<bool> UpdateBookAsync(int id, Book book)
        {
            logger.LogInformation("UpdateBookAsync requested.");
            try
            {
                var existingBook = await dbContext.Books.FindAsync(id);
                if (existingBook is null)
                {
                    logger.LogError("UpdateBookAsync failed.");
                    return false;
                }

                dbContext.Entry(existingBook).CurrentValues.SetValues(book);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("UpdateBookAsync successful.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "UpdateBookAsync failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteBookAsync(int id)
        {
            logger.LogInformation("DeleteBookAsync requested.");
            try
            {
                var result = await DbOperationHelper.ExecuteDeleteWithCountAsync(dbContext.Books, book => book.Id == id);
                if (result is (false, 0))
                {
                    logger.LogError("DeleteBookAsync failed.");
                    return (false, 0);
                }

                logger.LogInformation("DeleteBookAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "DeleteBookAsync failed.");
                throw;
            }
        }
    }
}
