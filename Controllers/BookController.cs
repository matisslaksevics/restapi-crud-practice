using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SBook;
namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController(IBookService bookService, ILogger<BookController> logger) : ControllerBase
    {
        const string GetBookEndpointName = "GetBook";

        // GET /Books
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<BookSummaryDto>>> GetAllBooks()
        {
            logger.LogInformation(
                "GET AllBooks requested by user {UserId} from IP {RemoteIp}",
                User.FindFirst("sub")?.Value,
                HttpContext.Connection.RemoteIpAddress);

            try { 
                var books = await bookService.GetAllBooksAsync();

                logger.LogInformation(
                    "GET AllBooks successful. Returned {BookCount} books",
                    books.Count);
                return books;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "GET AllBooks failed for user {UserId}",
                    User.FindFirst("sub")?.Value);
                throw;
            }
        } 

        // GET/Books/{id}
        [Authorize]
        [HttpGet("{id}", Name = GetBookEndpointName)]
        public async Task<ActionResult<BookDetailsDto>> GetBookById(int id)
        {
            logger.LogInformation(
                "GET GetBookById requested for ID {BookId} by user {UserId}",
                id,
                User.FindFirst("sub")?.Value);

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
                        "GET GetBookById successful for ID {BookId}",
                        id);
                    return Ok(book.ToBookDetailsDto());
                }
                else
                { 
                    logger.LogWarning(
                        "GET GetBookById failed -  not found book with ID {BookId}",
                        id);
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
                throw;
            }
        }

        // POST /Books
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/new-book")]
        public async Task<ActionResult<BookSummaryDto>> CreateBook([FromBody] CreateBookDto newBook) 
        {
            logger.LogInformation(
                "POST NewBook requested by user {UserId} from IP {RemoteIp}",
                User.FindFirst("sub")?.Value,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                var book = await bookService.CreateBookAsync(newBook);
                if (book is not null)
                {
                    logger.LogInformation(
                        "POST NewBook successful.");
                    return Ok(book);
                }
                else
                {
                    logger.LogWarning(
                            "POST NewBook failed. No entity received or created.");
                    return BadRequest("Failed book creation.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Post NewBook failed for user {UserId}",
                    User.FindFirst("sub")?.Value);
                throw;
            }
        }

        // PUT /Books/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/update-book/{id}")]
        public async Task<ActionResult<BookSummaryDto>> UpdateBook(int id, [FromBody] UpdateBookDto updatedBook)
        {
            logger.LogInformation(
                "PUT UpdateBook requested by user {UserId} from IP {RemoteIp}",
                User.FindFirst("sub")?.Value,
                HttpContext.Connection.RemoteIpAddress);
            try
            {
                var book = await bookService.UpdateBookAsync(id, updatedBook);
                if (book is not null)
                {
                    logger.LogInformation(
                        "PUT UpdateBook successful.");
                    return Ok(book);
                }
                else
                {
                    logger.LogWarning(
                            "PUT UpdateBook failed. No entity changed.");
                    return NotFound("Failed book creation.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Post NewBook failed for user {UserId}",
                    User.FindFirst("sub")?.Value);
                throw;
            }
        }

        // DELETE /Books/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            logger.LogInformation(
               "DELETE DeleteBook requested for ID {BookId} by user {UserId}",
               id,
               User.FindFirst("sub")?.Value);

            try
            {
                var (success, rowsAffected) = await bookService.DeleteBookAsync(id);

                if (!success)
                {
                    logger.LogWarning(
                        "DELETE DeleteBook failed - Book ID {BookId} not found",
                        id);
                    return NotFound($"Book with ID {id} not found.");
                }

                logger.LogInformation(
                    "DELETE DeleteClient successful for ID {BookId}. Rows affected: {RowsAffected}",
                    id, rowsAffected);

                return Ok(new { Message = $"Successfully deleted {rowsAffected} books(s)." });
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "DELETE DeleteBook failed for ID {BookId}",
                    id);
                throw;
            }
        }
    }
}