 using restapi_crud_practice.Mapping;
using restapi_crud_practice.Dtos.Client;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Services.SClient;
namespace restapi_crud_practice.Endpoints
{
    [Route("[controller]")]
    [ApiController]
    public class ClientEndpoints(IClientService clientService) : ControllerBase
    { 
        const string GetClientEndpointName = "GetClient";

        // GET /clients
        [HttpGet]
        public async Task<ActionResult<List<ClientSummaryDto>>> GetAllClients() => await clientService.GetAllClientsAsync();

        // GET/clients/{id}
        [HttpGet("{id}", Name = GetClientEndpointName)]
        public async Task<ActionResult<ClientDetailsDto>> GetClientById(int id)
        {
            var client = await clientService.GetClientByIdAsync(id);
            return client is null ? NotFound() : Ok(client.ToClientDetailsDto());
        }

        // POST /clients
        [HttpPost]
        public async Task<ActionResult<ClientSummaryDto>> CreateClient([FromBody] CreateClientDto newClient)
        {
            var createdClient = await clientService.CreateClientAsync(newClient);
            return CreatedAtRoute(GetClientEndpointName, new { id = createdClient.Id }, createdClient);
        }

        // PUT /clients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto updatedClient)
        {
            return await clientService.UpdateClientAsync(id, updatedClient) ? NoContent() : NotFound();
        }
        // DELETE /clients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        { 
            return await clientService.DeleteClientAsync(id) ? NoContent() : NotFound();
        }
    }
}