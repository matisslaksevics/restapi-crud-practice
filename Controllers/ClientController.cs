using restapi_crud_practice.Dtos.Client;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Services.SClient;
using restapi_crud_practice.Services.SUserContext;
using Microsoft.AspNetCore.Authorization;

namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController(IClientService clientService, ILogger<ClientController> logger, IUserContextService userContext) : ControllerBase
    {
        // GET /clients
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all-clients")]
        public async Task<ActionResult<List<ClientSummaryDto>>> GetAllClients()
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "GET AllClients requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);

            try
            {
                var clients = await clientService.GetAllClientsAsync();

                logger.LogInformation(
                    "GET AllClients successful. Returned {ClientCount} clients",
                    clients.Count);

                return clients;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GET AllClients failed for user {UserId}",
                    clientId);
                throw;
            }
        }

        // PUT /clients/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/edit-client/{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientDto updatedClient)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "PUT UpdateClient requested for ID {ClientId} by user {UserId}",
                id,
                clientId);
            try
            {
                var success = await clientService.UpdateClientAsync(id, updatedClient);

                if (success)
                {
                    logger.LogInformation(
                        "PUT UpdateClient successful for ID {ClientId}",
                        id);
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "PUT UpdateClient failed for ID {ClientId}",
                    id);
                throw;
            }
        }

        // DELETE /clients/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-client/{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "DELETE DeleteClient requested for ID {ClientId} by user {UserId}",
                id,
                clientId);

            try
            {
                var (success, rowsAffected) = await clientService.DeleteClientAsync(id);

                if (!success)
                {
                    return NotFound($"Client with ID {id} not found.");
                }

                logger.LogInformation(
                    "DELETE DeleteClient successful for ID {ClientId}. Rows affected: {RowsAffected}",
                    id, rowsAffected);

                return Ok(new { Message = $"Successfully deleted {rowsAffected} client(s)." });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "DELETE DeleteClient failed for ID {ClientId}",
                    id);
                throw;
            }
        }
    }
}