using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SBorrow;
using restapi_crud_practice.Services.SUserContext;
namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BorrowController(IBorrowService borrowService, IUserContextService userContext, ILogger<BorrowController> logger) : ControllerBase
    {
        const string GetBorrowEndpointName = "GetBorrow";
        // GET/Borrows
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all-borrows")]
        public async Task<ActionResult<List<BorrowSummaryDto>>> GetAllBorrows() 
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "GET AllBorrows requested by admin {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);

            try
            {
                var books = await borrowService.GetAllBorrowsAsync();

                logger.LogInformation(
                    "GET AllBorrows successful for admin {ClientId} Returned {BorrowCount} borrows",
                    clientId, books.Count);
                return books;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GET AllBorrows failed for admin {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for GET AllBorrows");
            }
        }

        // GET/Borrows/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/get-borrow/{id}", Name = GetBorrowEndpointName)]
        public async Task<ActionResult<BorrowDetailsDto>> GetBorrowById(int id)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "GET GetBorrowById requested by admin {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);

            try
            {
                var borrow = await borrowService.GetBorrowByIdAsync(id);
                if (borrow is null)
                {
                    return NotFound();
                }
                else
                {
                    logger.LogInformation("GET GetBorrowById successful for admin {ClientId}", clientId);
                    return Ok(borrow.ToBorrowDetailsDto());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GET GetBorrowById failed for admin {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for GET GetBorrowById");
            }
        }

        // GET/Borrows/user/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/user-borrows/{id:guid}")]
        public async Task<ActionResult<List<BorrowSummaryDto>>> GetAllClientBorrows(Guid id)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "GET UserBorrows requested by admin {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);

            try
            {
                var result = await borrowService.GetAllClientBorrowsAsync(id);
                if (result?.Any() == true)
                {
                    logger.LogInformation("GET UserBorrows successful for admin {ClientId}", clientId);
                    return Ok(result);
                }
                else
                {
                    return NotFound("No borrow records found for this user.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GET UserBorrows failed for admin {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for GET UserBorrows");
            }
        }

        // GET/Borrows/myborrows
        [Authorize]
        [HttpGet("myborrows")]
        public async Task<ActionResult<List<BorrowSummaryDto>>> GetMyBorrows()
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "GET MyBorrows requested by user {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);

            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }
                else
                {
                    logger.LogInformation("GET MyBorrows successful for user {ClientId}", clientId);
                    return await borrowService.GetAllClientBorrowsAsync(clientId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GET MyBorrows failed for user {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for GET MyBorrows");
            }
        }

        // POST/Borrows
        [Authorize]
        [HttpPost("new-borrow")]
        public async Task<ActionResult<BorrowSummaryDto>> CreateBorrow([FromBody] CreateUserBorrowDto newBorrow)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "POST CreateBorrow requested by user {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (clientId == null)
                {
                    return Unauthorized("Could not determine user from token.");
                }
                else
                {
                    var createdBorrow = await borrowService.CreateBorrowAsync(newBorrow, clientId);
                    logger.LogInformation("POST CreateBorrow successful for user {ClientId}", clientId);
                    return CreatedAtRoute(GetBorrowEndpointName, new { id = createdBorrow.Id }, createdBorrow);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST CreateBorrow failed for user {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for POST CreateBorrow");
            }
        }

        // POST/Borrows/admin/new-borrow
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/new-borrow")]
        public async Task<ActionResult<BorrowSummaryDto>> AdminCreateBorrow([FromBody] CreateBorrowDto newBorrow)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "POST AdminCreateBorrow requested by admin {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);
            try
            {
                var createdBorrow = await borrowService.AdminCreateBorrowAsync(newBorrow);
                if (createdBorrow is null)
                {
                    return BadRequest("Failed book creation.");
                }
                else
                {
                    logger.LogInformation("POST AdminCreateBorrow successful for admin {ClientId}", clientId);
                    return CreatedAtRoute(GetBorrowEndpointName, new { id = createdBorrow.Id }, createdBorrow);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST AdminCreateBorrow failed for admin {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for POST AdminCreateBorrow");
            }
        }

        // PUT/Borrows/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/edit-borrow/{id}")]
        public async Task<IActionResult> UpdateBorrow(int id, [FromBody] UpdateBorrowDto updatedBorrow)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "PUT UpdateBorrow requested by admin {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);
            try
            {
                if (await borrowService.UpdateBorrowAsync(id, updatedBorrow))
                {
                    logger.LogInformation("PUT UpdateBorrow successful admin {ClientId}", clientId);
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
                    "PUT UpdateBorrow failed for admin {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for PUT UpdateBorrow");
            }
        }

        // DELETE/Borrows/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-borrow/{id}")]
        public async Task<IActionResult> DeleteBorrow(int id)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "DELETE DeleteBorrow requested by admin {UserId} from IP {RemoteIp}",
               clientId,
               HttpContext.Connection.RemoteIpAddress);
            try
            {
                var (success, rowsAffected) = await borrowService.DeleteBorrowAsync(id);
                if (!success)
                {
                    return NotFound($"Borrow record with ID {id} not found.");
                } else
                {
                    logger.LogInformation("DELETE DeleteBorrow successful for admin {ClientId} Rows affected: {RowsAffected}", clientId, rowsAffected);
                    return Ok(new { Message = $"Successfully deleted {rowsAffected} borrow record(s)." });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "DELETE DeleteBorrow failed for admin {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for DELETE DeleteBorrow");
            }
        }
    }
}