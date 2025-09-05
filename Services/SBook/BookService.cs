using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Repositories.RBook;
namespace restapi_crud_practice.Services.SBook
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<List<BookSummaryDto>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
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
        public async Task<bool> UpdateBookAsync(int id, UpdateBookDto updatedBook)
        {
            var bookEntity = updatedBook.ToEntity(id);
            return await _bookRepository.UpdateBookAsync(id, bookEntity);
        }
        public async Task<bool> DeleteBookAsync(int id)
        {
            return await _bookRepository.DeleteBookAsync(id);
        }
    }
}
