using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories.RBook
{
    public interface IBookRepository
    {
        Task<List<BookSummaryDto>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(Book book);
        Task<bool> UpdateBookAsync(int id, Book book);
        Task<bool> DeleteBookAsync(int id);
    }
}
