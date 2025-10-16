using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using restapi_crud_practice.Dtos.Client;
using restapi_crud_practice.Services.SBook;
using restapi_crud_practice.Services.SClient;
using restapi_crud_practice.Services.SMyLogger;
namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController(IClientService clientService, IMyLoggerService logger) : ControllerBase
    {
        // GET /clients
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all-clients")]
        public async Task<ActionResult<List<ClientSummaryDto>>> GetAllClients()
        {
            logger.LogInfo("GET AllClients requested.");
            try
            {
                var clients = await clientService.GetAllClientsAsync();
                logger.LogInfo($"GET AllClients successful. Retrieved {clients.Count} clients");
                return Ok(clients);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GET AllClients failed.");
                return StatusCode(500, "Internal server error from GET AllClients");
            }
        }

        // PUT /clients/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/edit-client/{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientDto updatedClient)
        {
            logger.LogInfo("PUT UpdateClient requested.");
            try
            {
                var update = await clientService.UpdateClientAsync(id, updatedClient);
                if (!update)
                {
                    logger.LogError("PUT UpdateClient failed.");
                    return NotFound();
                }
                logger.LogInfo("PUT UpdateClient successful.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "PUT UpdateClient failed.");
                return StatusCode(500, "Internal server error from PUT UpdateClient");
            }
        }
        // DELETE /clients/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-client/{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            logger.LogInfo("DELETE DeleteClient requested.");
            try
            {
                var (success, rowsAffected) = await clientService.DeleteClientAsync(id);
                if (!success)
                {
                    logger.LogError("DELETE DeleteClient failed.");
                    return NotFound($"Client with ID {id} not found.");
                }
                logger.LogInfo("DELETE DeleteClient successful.");
                return Ok(new { Message = $"Successfully deleted {rowsAffected} client(s)." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DELETE DeleteClient failed.");
                return StatusCode(500, "Internal server error from DELETE DeleteClient");
            }
        }
    }
}