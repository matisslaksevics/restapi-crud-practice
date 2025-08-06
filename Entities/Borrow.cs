namespace restapi_crud_practice.Entities
{
    public class Borrow
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }
}