using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SBook;
using restapi_crud_practice.Services.SUserContext;
namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController(IBookService bookService, ILogger<BookController> logger, IUserContextService userContext) : ControllerBase
    {
        const string GetBookEndpointName = "GetBook";

        // GET /Books
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<BookSummaryDto>>> GetAllBooks()
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "GET AllBooks requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);

            try { 
                var books = await bookService.GetAllBooksAsync();

                logger.LogInformation(
                    "GET AllBooks successful for user {UserId}. Returned {BookCount} books",
                    clientId, books.Count);
                return books;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GET AllBooks failed for user {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for GET AllBooks");
            }
        } 

        // GET/Books/{id}
        [Authorize]
        [HttpGet("{id}", Name = GetBookEndpointName)]
        public async Task<ActionResult<BookDetailsDto>> GetBookById(int id)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "GET GetBookById requested for ID {BookId} by user {UserId}",
                id,
                clientId);

            if (id <= 0)
            {
                logger.LogWarning(
                    "GET GetBookById invalid ID {BookId} requested by user {UserId}",
                    id,
                    User.FindFirst("sub")?.Value);
                return BadRequest("Invalid book ID.");
            }

            try
            {
                var book = await bookService.GetBookByIdAsync(id);
                if (book is not null)
                {
                    logger.LogInformation(
                        "GET GetBookById successful for user {UserId} and Book {BookId}",
                        clientId, id);
                    return Ok(book.ToBookDetailsDto());
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
                    "GET GetBookById failed for ID {BookId} by user {UserId}",
                    id,
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for GET GetBookById");
            }
        }

        // POST /Books
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/new-book")]
        public async Task<ActionResult<BookSummaryDto>> CreateBook([FromBody] CreateBookDto newBook) 
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "POST NewBook requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                var book = await bookService.CreateBookAsync(newBook);
                if (book is not null)
                {
                    logger.LogInformation(
                        "POST NewBook successful for user {UserId}", clientId);
                    return Ok(book);
                }
                else
                {
                    return BadRequest("Failed book creation.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "POST NewBook failed for user {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for POST NewBook");
            }
        }

        // PUT /Books/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/update-book/{id}")]
        public async Task<ActionResult<BookSummaryDto>> UpdateBook(int id, [FromBody] UpdateBookDto updatedBook)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
                "PUT UpdateBook requested by user {UserId} from IP {RemoteIp}",
                clientId,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                var book = await bookService.UpdateBookAsync(id, updatedBook);
                if (book is not null)
                {
                    logger.LogInformation(
                        "PUT UpdateBook successful for user {UserId} for book {BookId}", clientId, id);
                    return Ok(book);
                }
                else
                {
                    return NotFound("Failed book creation.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "PUT UpdateBook failed for user {UserId}",
                    User.FindFirst("sub")?.Value);
                return StatusCode(500, "An internal error occurred for PUT UpdateBook");
            }
        }

        // DELETE /Books/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var clientId = userContext.GetUserId(User);
            logger.LogInformation(
               "DELETE DeleteBook requested for ID {BookId} by user {UserId}",
               id,
               clientId);

            try
            {
                var (success, rowsAffected) = await bookService.DeleteBookAsync(id);

                if (!success)
                {
                    return NotFound($"Book with ID {id} not found.");
                }

                logger.LogInformation(
                    "DELETE DeleteBook successful for ID {BookId}. Rows affected: {RowsAffected}",
                    id, rowsAffected);

                return Ok(new { Message = $"Successfully deleted {rowsAffected} books(s)." });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "DELETE DeleteBook failed for ID {BookId}",
                    id);
                return StatusCode(500, "An internal error occurred for DELETE DeleteBook");
            }
        }
    }
}