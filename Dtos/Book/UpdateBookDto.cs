using System.ComponentModel.DataAnnotations;

namespace restapi_crud_practice.Dtos.Book;
public record class UpdateBookDto
(
    [Required][MaxLength(100)] string BookName,
    [Required] DateOnly ReleaseDate
);