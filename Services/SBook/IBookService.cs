using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services.SBook
{
    public interface IBookService
    {
        Task<List<BookSummaryDto>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<BookSummaryDto> CreateBookAsync(CreateBookDto newBook);
        Task<BookSummaryDto?> UpdateBookAsync(int id, UpdateBookDto updatedBookDto);
        Task<(bool Success, int RowsAffected)> DeleteBookAsync(int id);
    }
}