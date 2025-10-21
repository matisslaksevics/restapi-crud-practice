namespace restapi_crud_practice.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; }

    }
}