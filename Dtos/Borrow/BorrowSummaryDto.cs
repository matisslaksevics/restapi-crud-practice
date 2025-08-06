namespace restapi_crud_practice.Dtos.Borrow;
public record class BorrowSummaryDto
(
    int Id,
    string Client,
    string Book,
    DateOnly BorrowDate,
    DateOnly? ReturnDate

);