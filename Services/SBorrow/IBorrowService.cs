using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services.SBorrow
{
    public interface IBorrowService
    {
        Task<List<BorrowSummaryDto>> GetAllBorrowsAsync();
        Task<Borrow?> GetBorrowByIdAsync(int id);
        Task<BorrowSummaryDto> CreateBorrowAsync(CreateBorrowDto newBorrow);
        Task<bool> UpdateBorrowAsync(int id, UpdateBorrowDto updatedBorrow);
        Task<int> DeleteBorrowAsync(int id);
    }
}