using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Services.SBorrow;
using restapi_crud_practice.Helpers;
namespace restapi_crud_practice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BorrowController(IBorrowService borrowService) : ControllerBase
    {
        const string GetBorrowEndpointName = "GetBorrow";
        // GET/Borrows
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all-borrows")]
        public async Task<ActionResult<List<BorrowSummaryDto>>> GetAllBorrows() => await borrowService.GetAllBorrowsAsync();

        // GET/Borrows/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/get-borrow/{id}", Name = GetBorrowEndpointName)]
        public async Task<ActionResult<BorrowDetailsDto>> GetBorrowById(int id)
        {
            var borrow = await borrowService.GetBorrowByIdAsync(id);
            return borrow is null ? NotFound() : Ok(borrow.ToBorrowDetailsDto());
        }

        // GET/Borrows/user/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/user-borrows/{id}")]
        public async Task<ActionResult<List<BorrowSummaryDto>>> GetAllClientBorrows(Guid id) => await borrowService.GetAllClientBorrowsAsync(id);

        // GET/Borrows/myborrows
        [Authorize]
        [HttpGet("myborrows")]
        public async Task<ActionResult<List<BorrowSummaryDto>>> GetMyBorrows()
        {
            var clientId = UserHelper.GetUserId(User);
            if (clientId == null) return Unauthorized("Could not determine user from token.");
            return await borrowService.GetAllClientBorrowsAsync(clientId);
        }

        // POST/Borrows
        [Authorize]
        [HttpPost("new-borrow")]
        public async Task<ActionResult<BorrowSummaryDto>> CreateBorrow([FromBody] CreateUserBorrowDto newBorrow)
        {
            var clientId = UserHelper.GetUserId(User);
            if (clientId == null)
            {
                return Unauthorized("Could not determine user from token.");
            }
            var createdBorrow = await borrowService.CreateBorrowAsync(newBorrow, clientId);
            return CreatedAtRoute(GetBorrowEndpointName, new { id = createdBorrow.Id }, createdBorrow);
        }

        // POST/Borrows/admin/new-borrow
        [Authorize]
        [HttpPost("admin/new-borrow")]
        public async Task<ActionResult<BorrowSummaryDto>> AdminCreateBorrow([FromBody] CreateBorrowDto newBorrow)
        {
            var createdBorrow = await borrowService.AdminCreateBorrowAsync(newBorrow);
            return CreatedAtRoute(GetBorrowEndpointName, new { id = createdBorrow.Id }, createdBorrow);
        }

        // PUT/Borrows/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/edit-borrow/{id}")]
        public async Task<IActionResult> UpdateBorrow(int id, [FromBody] UpdateBorrowDto updatedBorrow)
        {
            return await borrowService.UpdateBorrowAsync(id, updatedBorrow) ? NoContent() : NotFound();
        }

        // DELETE/Borrows/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete-borrow/{id}")]
        public async Task<IActionResult> DeleteBorrow(int id) 
        {
            return await borrowService.DeleteBorrowAsync(id) ? NoContent() : NotFound();
        }
    }
}