using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Mapping
{
    public static class BookMapping
    {
        public static BookSummaryDto ToBookSummaryDto(this Book book)
        {
            return new(
                book.Id,
                book.BookName,
                book.ReleaseDate
            );
        }
        public static BookDetailsDto ToBookDetailsDto(this Book book)
        {
            return new(
                book.Id,
                book.BookName,
                book.ReleaseDate
            );
        }
        public static Book ToEntity(this CreateBookDto book)
        {
            return new Book()
            {
                BookName = book.BookName,
                ReleaseDate = book.ReleaseDate
            };
        }
        public static Book ToEntity(this UpdateBookDto book, int id)
        {
            return new Book()
            {
                Id = id,
                BookName = book.BookName,
                ReleaseDate = book.ReleaseDate
            };
        }
    }
}