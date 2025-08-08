using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SBook;
using System.Net;
namespace restapi_crud_practice.Endpoints
{
    public static class BookEndpoints
    {
        const string GetBookEndpointName = "GetBook";
        public static RouteGroupBuilder MapBookEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("books");

            // GET /Books
            group.MapGet("/", async (IBookService bookService) => await bookService.GetAllBooksAsync());

            // GET/Books/{id}
            group.MapGet("/{id}", async (int id, IBookService bookService) =>
            {
                var book = await bookService.GetBookByIdAsync(id);
                return book is null ? Results.NotFound() : Results.Ok(book.ToBookDetailsDto());

            }).WithName(GetBookEndpointName);

            // POST /Books
            group.MapPost("/", async ([FromBody]CreateBookDto newBook, [FromServices]IBookService bookService) =>
            {
                var createdBook = await bookService.CreateBookAsync(newBook);
                return Results.CreatedAtRoute(GetBookEndpointName, new { id = createdBook.Id }, createdBook);
            });

            // PUT /Books/{id}
            group.MapPut("/{id}", async (int id, [FromBody]UpdateBookDto updatedBook, [FromServices]IBookService bookService) =>
            {
                return await bookService.UpdateBookAsync(id, updatedBook) ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /Books/{id}
            group.MapDelete("/{id}", async (int id, IBookService bookService) =>
            {
                await bookService.DeleteBookAsync(id);
                return Results.NoContent();
            });
            return group;
        }
    }
}