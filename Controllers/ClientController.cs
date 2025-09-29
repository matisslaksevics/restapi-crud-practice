using restapi_crud_practice.Dtos.Client;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Services.SClient;
using Microsoft.AspNetCore.Authorization;

namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController(IClientService clientService, ILogger<ClientController> logger) : ControllerBase
    {

        // GET /clients
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all-clients")]
        public async Task<ActionResult<List<ClientSummaryDto>>> GetAllClients()
        {
            logger.LogInformation(
                "GET AllClients requested by user {UserId} from IP {RemoteIp}",
                User.FindFirst("sub")?.Value,
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
                    User.FindFirst("sub")?.Value);
                throw;
            }
        }

        // PUT /clients/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/edit-client/{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientDto updatedClient)
        {
            logger.LogInformation(
                "PUT UpdateClient requested for ID {ClientId} by user {UserId}",
                id,
                User.FindFirst("sub")?.Value);

            if (!ModelState.IsValid)
            {
                logger.LogWarning(
                    "PUT UpdateClient invalid model state for ID {ClientId}. Errors: {ModelErrors}",
                    id,
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

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
                    logger.LogWarning(
                        "PUT UpdateClient failed - Client ID {ClientId} not found",
                        id);
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
            logger.LogInformation(
                "DELETE DeleteClient requested for ID {ClientId} by user {UserId}",
                id,
                User.FindFirst("sub")?.Value);

            try
            {
                var (success, rowsAffected) = await clientService.DeleteClientAsync(id);

                if (!success)
                {
                    logger.LogWarning(
                        "DELETE DeleteClient failed - Client ID {ClientId} not found",
                        id);
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