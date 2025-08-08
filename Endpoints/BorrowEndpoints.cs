using restapi_crud_practice.Mapping;
using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Services.SBorrow;
namespace restapi_crud_practice.Endpoints
{
    public static class BorrowEndpoints
    {

        const string GetBorrowEndpointName = "GetBorrow";
        public static RouteGroupBuilder MapBorrowEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("borrows");

            // GET /Borrows
            group.MapGet("/", async (IBorrowService borrowService) => await borrowService.GetAllBorrowsAsync());

            // GET/Borrows/{id}
            group.MapGet("/{id}", async (int id, IBorrowService borrowService) =>
            {
                var borrow = await borrowService.GetBorrowByIdAsync(id);
                return borrow is null ? Results.NotFound() : Results.Ok(borrow.ToBorrowDetailsDto());

            }).WithName(GetBorrowEndpointName);

            // POST /Borrows
            group.MapPost("/", async (CreateBorrowDto newBorrow, IBorrowService borrowService) =>
            {
                var createdBorrow = await borrowService.CreateBorrowAsync(newBorrow);
                return Results.CreatedAtRoute(GetBorrowEndpointName, new { id = createdBorrow.Id }, createdBorrow);
            });

            // PUT /Borrows/{id}
            group.MapPut("/{id}", async (int id, UpdateBorrowDto updatedBorrow, IBorrowService borrowService) =>
            {
                return await borrowService.UpdateBorrowAsync(id, updatedBorrow) ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /Borrows/{id}
            group.MapDelete("/{id}", async (int id, IBorrowService borrowService) =>
            {
                await borrowService.DeleteBorrowAsync(id);
                return Results.NoContent();
            });
            return group;
        }
    }
}