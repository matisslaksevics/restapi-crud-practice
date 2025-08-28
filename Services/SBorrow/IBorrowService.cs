using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services.SBorrow
{
    public interface IBorrowService
    {
        Task<List<BorrowSummaryDto>> GetAllBorrowsAsync();
        Task<Borrow?> GetBorrowByIdAsync(int id);
        Task<List<BorrowSummaryDto>> GetAllClientBorrowsAsync(Guid userId);
        Task<BorrowSummaryDto> AdminCreateBorrowAsync(CreateBorrowDto newBorrow);
        Task<BorrowSummaryDto> CreateBorrowAsync(CreateBorrowDto newBorrow, Guid ClientId);
        Task<bool> UpdateBorrowAsync(int id, UpdateBorrowDto updatedBorrow);
        Task<bool> DeleteBorrowAsync(int id);
    }
}