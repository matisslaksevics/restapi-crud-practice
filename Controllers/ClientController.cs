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
                "GET AllClients requested by admin {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);

            try
            {
                var clients = await clientService.GetAllClientsAsync();

                logger.LogInformation(
                    "GET AllClients successful for admin {UserId}. Returned {ClientCount} clients", 
                    clientId,
                    clients.Count);

                return clients;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GET AllClients failed for admin {UserId}",
                    clientId);
                return StatusCode(500, "An internal error occurred for GET AllClients");
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
                return StatusCode(500, "An internal error occurred for PUT UpdateClient");
            }
        }

        // DELETE /clients/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-client/{id}")]
        public async Task<IActionResult> DeleteClient(Guid clientId)
        {
            var adminId = userContext.GetUserId(User);
            logger.LogInformation(
                "DELETE DeleteClient requested for ID {ClientId} by admin {AdminId}",
                clientId,
                adminId);

            try
            {
                var (success, rowsAffected) = await clientService.DeleteClientAsync(clientId);

                if (!success)
                {
                    return NotFound($"Client with ID {clientId} not found.");
                }

                logger.LogInformation(
                    "DELETE DeleteClient successful for admin {AdminId}. Rows affected: {RowsAffected}",
                    clientId, rowsAffected);

                return Ok(new { Message = $"Successfully deleted {rowsAffected} client(s)." });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "DELETE DeleteClient failed for admin {AdminId}",
                    adminId);
                return StatusCode(500, "An internal error occurred for DELETE DeleteClient");
            }
        }
    }
}