using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SBook;
using restapi_crud_practice.Services.SMyLogger;
namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController(IBookService bookService, IMyLoggerService logger) : ControllerBase
    {
        const string GetBookEndpointName = "GetBook";

        // GET /Books
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<BookSummaryDto>>> GetAllBooks()
        {
            logger.LogInfo("GET AllBooks requested.");
            try
            {
                var books = await bookService.GetAllBooksAsync();
                logger.LogInfo($"GET AllBooks successful. Retrieved {books.Count} books");
                return Ok(books);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GET AllBooks failed.");
                return StatusCode(500, "Internal server error from GET AllBooks");
            }
        }

        // GET/Books/{id}
        [Authorize]
        [HttpGet("{id}", Name = GetBookEndpointName)]
        public async Task<ActionResult<BookDetailsDto>> GetBookById(int id)
        {
            logger.LogInfo("GET BookById requested.");
            try
            {
                var book = await bookService.GetBookByIdAsync(id);
                if (book is null)
                {
                    logger.LogError("GET BookById failed.");
                    return NotFound();
                }
                logger.LogInfo("GET BookById successful.");
                return Ok(book.ToBookDetailsDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GET BookById failed.");
                return StatusCode(500, "Internal server error from GET BookById");
            }
        }

        // POST /Books
        [Authorize(Roles = "Admin")]
        [HttpPost("admin/new-book")]
        public async Task<ActionResult<BookSummaryDto>> CreateBook([FromBody] CreateBookDto newBook) 
        {
            logger.LogInfo("POST CreateBook requested.");
            try
            {
                var createdBook = await bookService.CreateBookAsync(newBook);
                logger.LogInfo("POST CreateBook successful.");
                return Ok(createdBook);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "POST CreateBook failed.");
                return StatusCode(500, "Internal server error from POST CreateBook");
            }
        }

        // PUT /Books/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/update-book/{id}")]
        public async Task<ActionResult<BookSummaryDto>> UpdateBook(int id, [FromBody] UpdateBookDto updatedBook)
        {
            logger.LogInfo("PUT UpdateBook requested.");
            try
            {
                var result = await bookService.UpdateBookAsync(id, updatedBook);
                if (result is null)
                {
                    logger.LogError("PUT UpdateBook failed.");
                    return NotFound();
                }
                logger.LogInfo("PUT UpdateBook successful.");
                return Ok(result);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "PUT UpdateBook failed.");
                return StatusCode(500, "Internal server error from PUT UpdateBook");
            }
        }

        // DELETE /Books/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            logger.LogInfo("DELETE DeleteBook requested.");
            try
            {
                var (success, rowsAffected) = await bookService.DeleteBookAsync(id);
                if (!success)
                {
                    logger.LogError("DELETE DeleteBook failed.");
                    return NotFound();
                }
                logger.LogInfo("DELETE DeleteBook successful.");
                return Ok(new { Message = $"Successfully deleted {rowsAffected} book(s)." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DELETE DeleteBook failed.");
                return StatusCode(500, "Internal server error from DELETE DeleteBook");
            }
        }
    }
}