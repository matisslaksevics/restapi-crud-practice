using System.ComponentModel.DataAnnotations;

namespace restapi_crud_practice.Dtos.Borrow;
public record class CreateBorrowDto
(
    int ClientId,
    int BookId,
    [Required]DateOnly BorrowDate,
    DateOnly? ReturnDate,
    bool? IsOverdue
);
