using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Entities;
namespace restapi_crud_practice.Data
{
	public class BookBorrowingContext(DbContextOptions<BookBorrowingContext> options) : DbContext(options)
	{
		public DbSet<Client> Clients => Set<Client>();
		public DbSet<Book> Books => Set<Book>();
		public DbSet<Borrow> Borrows => Set<Borrow>();
    }
}

