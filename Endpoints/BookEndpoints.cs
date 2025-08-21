using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Book;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SBook;
using restapi_crud_practice.Services.SClient;
namespace restapi_crud_practice.Endpoints
{
    [Route("[controller]")]
    [ApiController]
    public class BookEndpoints(IBookService bookService) : ControllerBase
    {
        const string GetBookEndpointName = "GetBook";

        // GET /Books
        [HttpGet]
        public async Task<ActionResult<List<BookSummaryDto>>> GetAllBooks() => await bookService.GetAllBooksAsync();

        // GET/Books/{id}
        [HttpGet("{id}", Name = GetBookEndpointName)]
        public async Task<ActionResult<BookDetailsDto>> GetBookById(int id)
        {
             var book = await bookService.GetBookByIdAsync(id);
             return book is null ? NotFound() : Ok(book.ToBookDetailsDto());
        }

        // POST /Books
        [HttpPost]
        public async Task<ActionResult<BookSummaryDto>> CreateBook([FromBody] CreateBookDto newBook) 
        { 
            var createdBook = await bookService.CreateBookAsync(newBook);
            return CreatedAtRoute(GetBookEndpointName, new { id = createdBook.Id }, createdBook);
        }

        // PUT /Books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto updatedBook)
        {
            return await bookService.UpdateBookAsync(id, updatedBook) ? NoContent() : NotFound();
        }

        // DELETE /Books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id) 
        {
            return await bookService.DeleteBookAsync(id) ? NoContent() : NotFound();
        }
    }
}