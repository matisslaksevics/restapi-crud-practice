using restapi_crud_practice.Mapping;
using restapi_crud_practice.Dtos.Client;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Services.SClient;
namespace restapi_crud_practice.Endpoints
{
    public static class ClientEndpoints
    {
        const string GetClientEndpointName = "GetClient";
        public static RouteGroupBuilder MapClientEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("clients");

            // GET /clients
            group.MapGet("/", async (IClientService clientService) => await clientService.GetAllClientsAsync());

            // GET/clients/{id}
            group.MapGet("/{id}", async (int id, IClientService clientService) => 
            {
                var client = await clientService.GetClientByIdAsync(id);
                return client is null ? Results.NotFound() : Results.Ok(client.ToClientDetailsDto());

            }).WithName(GetClientEndpointName);

            // POST /clients
            group.MapPost("/", async ([FromBody]CreateClientDto newClient, [FromServices]IClientService clientService) => 
            { 
                var createdClient = await clientService.CreateClientAsync(newClient);
                return Results.CreatedAtRoute(GetClientEndpointName, new { id = createdClient.Id }, createdClient);
            });

            // PUT /clients/{id}
            group.MapPut("/{id}", async (int id, [FromBody]UpdateClientDto updatedClient, [FromServices]IClientService clientService) =>
            {
                return await clientService.UpdateClientAsync(id, updatedClient) ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /clients/{id}
            group.MapDelete("/{id}", async (int id, IClientService clientService) => 
            {
                await clientService.DeleteClientAsync(id);
                return Results.NoContent();
            });
            return group;
        }
    }
}