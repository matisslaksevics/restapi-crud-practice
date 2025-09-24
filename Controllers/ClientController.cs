using restapi_crud_practice.Dtos.Client;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Services.SClient;
using Microsoft.AspNetCore.Authorization;
namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController(IClientService clientService) : ControllerBase
    { 
        // GET /clients
        [Authorize(Roles="Admin")]
        [HttpGet("admin/all-clients")]
        public async Task<ActionResult<List<ClientSummaryDto>>> GetAllClients() => await clientService.GetAllClientsAsync();

        // PUT /clients/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/edit-client/{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientDto updatedClient)
        {
            return await clientService.UpdateClientAsync(id, updatedClient) ? NoContent() : NotFound();
        }
        // DELETE /clients/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-client/{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var (success, rowsAffected) = await clientService.DeleteClientAsync(id);
            if (!success)
            {
                return NotFound($"Client with ID {id} not found.");
            }
            return Ok(new { Message = $"Successfully deleted {rowsAffected} client(s)." });
        }
    }
}