using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Repositories.RBook;
using restapi_crud_practice.Services.SMyLogger;
namespace restapi_crud_practice.Services.SBook
{
    public class BookService(IBookRepository bookRepository, IMyLoggerService logger) : IBookService
    {
        private readonly IBookRepository _bookRepository = bookRepository;

        public async Task<List<BookSummaryDto>> GetAllBooksAsync()
        {
            logger.LogInfo("Service: GetAllBooks requested.");
            try
            {
                var books = await _bookRepository.GetAllBooksAsync();
                logger.LogInfo($"Service: GetAllBooks successful. Retrieved {books.Count} books.");
                return books.Select(book => book.ToBookSummaryDto()).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: GetAllBooks failed.");
                throw;
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            logger.LogInfo("Service: GetBookById requested.");
            try
            {
                var books = await _bookRepository.GetBookByIdAsync(id);
                logger.LogInfo("Service: GetBookById successful.");
                return books;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: GetBookById failed.");
                throw;
            }
        }
        public async Task<BookSummaryDto> CreateBookAsync(CreateBookDto newBook)
        {
            logger.LogInfo("Service: CreateBook requested.");
            try
            {
                var book = newBook.ToEntity();
                var createdBook = await _bookRepository.CreateBookAsync(book);
                logger.LogInfo("Service: CreateBook successful.");
                return createdBook.ToBookSummaryDto();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: CreateBook failed.");
                throw;
            }
        }
        public async Task<BookSummaryDto?> UpdateBookAsync(int id, UpdateBookDto updatedBookDto)
        {
            logger.LogInfo("Service: UpdateBook requested.");
            try
            {
                var bookEntity = updatedBookDto.ToEntity(id);
                var success = await _bookRepository.UpdateBookAsync(id, bookEntity);
                if (!success)
                {
                    logger.LogError("Service: UpdateBook failed.");
                    return null;
                }
                var updatedBook = await _bookRepository.GetBookByIdAsync(id);
                logger.LogInfo("Service: UpdateBook successful.");
                return updatedBook?.ToBookSummaryDto();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: UpdateBook failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteBookAsync(int id)
        {
            logger.LogInfo("Service: DeleteBook requested.");
            try
            {
                var result = await _bookRepository.DeleteBookAsync(id);
                logger.LogInfo("Service: DeleteBook successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Service: DeleteBook failed.");
                throw;
            }
        }
    }
}
