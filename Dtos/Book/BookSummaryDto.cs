namespace restapi_crud_practice.Dtos.Book;
public record class BookSummaryDto
(
    int Id,
    string BookName,
    DateOnly ReleaseDate
);