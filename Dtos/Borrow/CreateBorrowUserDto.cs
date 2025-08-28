using System.ComponentModel.DataAnnotations;

namespace restapi_crud_practice.Dtos.Borrow;
public record class CreateBorrowUserDto
(
    int BookId,
    [Required] DateOnly BorrowDate,
    DateOnly? ReturnDate,
    bool? IsOverdue
);
