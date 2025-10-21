namespace restapi_crud_practice.Dtos.Borrow;
public record class BorrowDetailsDto
(
    int Id,
    Guid ClientId,
    int BookId,
    DateOnly BorrowDate,
    DateOnly? ReturnDate,
    bool? IsOverdue
);