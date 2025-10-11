using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Repositories.RBook;
namespace restapi_crud_practice.Services.SBook
{
    public class BookService(IBookRepository bookRepository, ILogger<BookService> logger) : IBookService
    {
        private readonly IBookRepository _bookRepository = bookRepository;

        public async Task<List<BookSummaryDto>> GetAllBooksAsync()
        {
            logger.LogInformation("Service: GetAllBooksAsync requested.");
            try
            {
                var result = await _bookRepository.GetAllBooksAsync();
                logger.LogInformation("Service: GetAllBooksAsync successful.");
                return result.Select(book => book.ToBookSummaryDto()).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GetAllBooksAsync failed.");
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            logger.LogInformation("Service: GetBookByIdAsync requested.");
            try
            {
                var result = await _bookRepository.GetBookByIdAsync(id);

                if (result is not null)
                {
                    logger.LogInformation("Service: GetBookByIdAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Service: GetBookByIdAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GetBookByIdAsync failed.");
                throw;
            }
        }
        public async Task<BookSummaryDto> CreateBookAsync(CreateBookDto newBook)
        {
            logger.LogInformation("Service: CreateBookAsync requested.");
            try
            {
                var book = newBook.ToEntity();
                var createdBook = await _bookRepository.CreateBookAsync(book);
                logger.LogInformation("Service: CreateBookAsync successful.");
                return createdBook.ToBookSummaryDto();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: CreateBookAsync failed.");
                throw;
            }
        }
        public async Task<BookSummaryDto?> UpdateBookAsync(int id, UpdateBookDto updatedBookDto)
        {
            logger.LogInformation("Service: UpdateBookAsync requested.");
            try
            {
                var bookEntity = updatedBookDto.ToEntity(id);
                var success = await _bookRepository.UpdateBookAsync(id, bookEntity);

                if (success)
                {
                    var updatedBook = await _bookRepository.GetBookByIdAsync(id);
                    logger.LogInformation("Service: UpdateBookAsync successful.");
                    return updatedBook?.ToBookSummaryDto();
                }
                else
                {
                    logger.LogError("Service: UpdateBookAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: UpdateBookAsync failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteBookAsync(int id)
        {
            logger.LogInformation("Service: DeleteBookAsync requested.");
            try
            {
                var result = await _bookRepository.DeleteBookAsync(id);
                if (result is not (false, 0))
                {
                    logger.LogInformation("Service: DeleteBookAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Service: DeleteBookAsync failed.");
                    return (false, 0);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: DeleteBookAsync failed.");
                throw;
            }
        }
    }
}
