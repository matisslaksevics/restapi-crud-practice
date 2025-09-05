namespace restapi_crud_practice.Dtos.Borrow;
public record class CreateUserBorrowDto
{
    public int BookId { get; set; }
    public required DateOnly BorrowDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public bool? IsOverdue { get; internal set; }
}
