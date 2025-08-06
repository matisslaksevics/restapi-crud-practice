using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Entities;
namespace restapi_crud_practice.Endpoints
{
    public static class ClientEndpoints
    {
        const string GetClientEndpointName = "GetClient";
        public static RouteGroupBuilder MapClientEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("clients");

            // GET /clients
            group.MapGet("/", (BookBorrowingContext dbContext) => dbContext.Clients.Select(client => client.ToClientSummaryDto()).ToListAsync());

            // GET/clients/{id}
            group.MapGet("/{id}", async (int id, BookBorrowingContext dbContext) => 
            {
                Client? client = await dbContext.Clients.FindAsync(id);
                return client is null ? Results.NotFound() : Results.Ok(client.ToClientDetailsDto());

            }).WithName(GetClientEndpointName);

            // POST /clients
            group.MapPost("/", async (CreateClientDto newClient, BookBorrowingContext dbContext) => 
            { 
                Client client = newClient.ToEntity();
                dbContext.Clients.Add(client);
                await dbContext.SaveChangesAsync();
                return Results.CreatedAtRoute(GetClientEndpointName, new { id = client.Id }, client.ToClientSummaryDto());
            });

            // PUT /clients/{id}
            group.MapPut("/{id}", async (int id, UpdateClientDto updatedClient, BookBorrowingContext dbContext) =>
            {
                var existingClient = await dbContext.Clients.FindAsync(id);
                if (existingClient is null)
                {
                    return Results.NotFound();
                }
                dbContext.Entry(existingClient).CurrentValues.SetValues(updatedClient.ToEntity(id));
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            // DELETE /clients/{id}
            group.MapDelete("/{id}", async (int id, BookBorrowingContext dbContext) => 
            {
                await dbContext.Clients.Where(client => client.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            });
            return group;
        }
    }
}