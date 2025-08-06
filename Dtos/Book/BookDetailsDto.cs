namespace restapi_crud_practice.Dtos.Book;
public record class BookDetailsDto
(
    int Id,
    string BookName,
    DateOnly ReleaseDate
);
