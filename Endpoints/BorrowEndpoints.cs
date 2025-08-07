using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services;
using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;
namespace restapi_crud_practice.Endpoints
{
    public static class BorrowEndpoints
    {

        const string GetBorrowEndpointName = "GetBorrow";
        public static RouteGroupBuilder MapBorrowEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("borrows");

            // GET /Borrows
            group.MapGet("/", (BookBorrowingContext dbContext, IBorrowService borrowService) =>
            {
                return dbContext.Borrows.Where(borrow => borrow.IsOverdue == true).Include(borrow => borrow.Client).Include(borrow => borrow.Book).Select(borrow => borrow.ToBorrowSummaryDto()).ToListAsync();
            });

            // GET/Borrows/{id}
            group.MapGet("/{id}", async (int id, BookBorrowingContext dbContext) =>
            {
                Borrow? borrow = await dbContext.Borrows.FindAsync(id);
                return borrow is null ? Results.NotFound() : Results.Ok(borrow.ToBorrowDetailsDto());

            }).WithName(GetBorrowEndpointName);

            // POST /Borrows
            group.MapPost("/", async (CreateBorrowDto newBorrow, BookBorrowingContext dbContext) =>
            {
                Borrow borrow = newBorrow.ToEntity();
                borrow.Client = dbContext.Clients.Find(newBorrow.ClientId);
                borrow.Book = dbContext.Books.Find(newBorrow.BookId);
                if (borrow.ReturnDate is not null)
                {
                    if (borrow.BorrowDate.AddMonths(3) < borrow.ReturnDate)
                    {
                        borrow.IsOverdue = true;
                    }
                    else
                    {
                        borrow.IsOverdue = false;
                    }
                }
                dbContext.Borrows.Add(borrow);
                await dbContext.SaveChangesAsync();
                return Results.CreatedAtRoute(GetBorrowEndpointName, new { id = borrow.Id }, borrow.ToBorrowSummaryDto());
            });

            // PUT /Borrows/{id}
            group.MapPut("/{id}", async (int id, UpdateBorrowDto updatedBorrow, BookBorrowingContext dbContext) =>
            {
                var existingBorrow = await dbContext.Borrows.FindAsync(id);
                if (existingBorrow is null) return Results.NotFound();
                var modifiedBorrow = updatedBorrow with
                {
                    IsOverdue = updatedBorrow.ReturnDate is not null && updatedBorrow.BorrowDate.AddMonths(3) < updatedBorrow.ReturnDate
                };
                dbContext.Entry(existingBorrow).CurrentValues.SetValues(modifiedBorrow.ToEntity(id));
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            // DELETE /Borrows/{id}
            group.MapDelete("/{id}", async (int id, BookBorrowingContext dbContext) =>
            {
                await dbContext.Borrows.Where(borrow => borrow.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            });
            group.MapGet("/test", (BookBorrowingContext dbContext, IBorrowService borrowService) =>
            {
                return borrowService.GetAllBorrows();
            });
            return group;
        }
    }
}