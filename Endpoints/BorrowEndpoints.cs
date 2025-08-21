using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SBorrow;
namespace restapi_crud_practice.Endpoints
{
    [Route("[controller]")]
    [ApiController]
    public class BorrowEndpoints(IBorrowService borrowService) : ControllerBase
    {
        const string GetBorrowEndpointName = "GetBorrow";
        // GET /Borrows
        [HttpGet]
        public async Task<ActionResult<List<BorrowSummaryDto>>> GetAllBorrows() => await borrowService.GetAllBorrowsAsync();

        // GET/Borrows/{id}
        [HttpGet("{id}", Name = GetBorrowEndpointName)]
        public async Task<ActionResult<BorrowDetailsDto>> GetBorrowById(int id)
        {
            var borrow = await borrowService.GetBorrowByIdAsync(id);
            return borrow is null ? NotFound() : Ok(borrow.ToBorrowDetailsDto());
        }

        // POST /Borrows
        [HttpPost]
        public async Task<ActionResult<BorrowSummaryDto>> CreateBorrow([FromBody] CreateBorrowDto newBorrow)
        {
            var createdBorrow = await borrowService.CreateBorrowAsync(newBorrow);
            return CreatedAtRoute(GetBorrowEndpointName, new { id = createdBorrow.Id }, createdBorrow);
        }

        // PUT /Borrows/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrow(int id, [FromBody] UpdateBorrowDto updatedBorrow)
        {
            return await borrowService.UpdateBorrowAsync(id, updatedBorrow) ? NoContent() : NotFound();
        }

        // DELETE /Borrows/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrow(int id) 
        {
            return await borrowService.DeleteBorrowAsync(id) ? NoContent() : NotFound();
        }
    }
}