namespace restapi_crud_practice.Dtos.Borrow;
public record class BorrowDetailsDto
(
    int Id,
    int ClientId,
    int BookId,
    DateOnly BorrowDate,
    DateOnly? ReturnDate,
    bool? IsOverdue
);