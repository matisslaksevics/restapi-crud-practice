using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Repositories.RBook;
namespace restapi_crud_practice.Services.SBook
{
    public class BookService(IBookRepository bookRepository) : IBookService
    {
        private readonly IBookRepository _bookRepository = bookRepository;

        public async Task<List<BookSummaryDto>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return books.Select(book => book.ToBookSummaryDto()).ToList();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetBookByIdAsync(id);
        }
        public async Task<BookSummaryDto> CreateBookAsync(CreateBookDto newBook)
        {
            var book = newBook.ToEntity();
            var createdBook = await _bookRepository.CreateBookAsync(book);
            return createdBook.ToBookSummaryDto();
        }
        public async Task<BookSummaryDto?> UpdateBookAsync(int id, UpdateBookDto updatedBookDto)
        {
            var bookEntity = updatedBookDto.ToEntity(id);
            var success = await _bookRepository.UpdateBookAsync(id, bookEntity);
            if (!success)
            {
                return null;
            }

            var updatedBook = await _bookRepository.GetBookByIdAsync(id);
            return updatedBook?.ToBookSummaryDto();
        }
        public async Task<(bool Success, int RowsAffected)> DeleteBookAsync(int id)
        {
            return await _bookRepository.DeleteBookAsync(id);
        }
    }
}
