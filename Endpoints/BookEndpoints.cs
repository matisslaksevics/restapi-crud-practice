using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Entities;
namespace restapi_crud_practice.Endpoints
{
    public static class BookEndpoints
    {
        const string GetBookEndpointName = "GetBook";
        public static RouteGroupBuilder MapBookEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("books");

            // GET /Books
            group.MapGet("/", (BookBorrowingContext dbContext) => dbContext.Books.Select(book => book.ToBookSummaryDto()).ToListAsync());

            // GET/Books/{id}
            group.MapGet("/{id}", async (int id, BookBorrowingContext dbContext) =>
            {
                Book? book = await dbContext.Books.FindAsync(id);
                return book is null ? Results.NotFound() : Results.Ok(book.ToBookDetailsDto());

            }).WithName(GetBookEndpointName);

            // POST /Books
            group.MapPost("/", async (CreateBookDto newBook, BookBorrowingContext dbContext) =>
            {
                Book book = newBook.ToEntity();
                dbContext.Books.Add(book);
                await dbContext.SaveChangesAsync();
                return Results.CreatedAtRoute(GetBookEndpointName, new { id = book.Id }, book.ToBookSummaryDto());
            });

            // PUT /Books/{id}
            group.MapPut("/{id}", async (int id, UpdateBookDto updatedBook, BookBorrowingContext dbContext) =>
            {
                var existingBook = await dbContext.Books.FindAsync(id);
                if (existingBook is null)
                {
                    return Results.NotFound();
                }
                dbContext.Entry(existingBook).CurrentValues.SetValues(updatedBook.ToEntity(id));
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            // DELETE /Books/{id}
            group.MapDelete("/{id}", async (int id, BookBorrowingContext dbContext) =>
            {
                await dbContext.Books.Where(book => book.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            });
            return group;
        }
    }
}