using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Repositories.RBorrow
{
    public interface IBorrowRepository
    {
        Task<List<BorrowSummaryDto>> GetAllBorrowsAsync();
        Task<Borrow?> GetBorrowByIdAsync(int id);
        Task<List<BorrowSummaryDto>> GetAllClientBorrowsAsync(Guid? userId);
        Task<Borrow?> CreateBorrowAsync(Borrow borrow); 
        Task<bool> UpdateBorrowAsync(int id, Borrow borrow);
        Task<bool> DeleteBorrowAsync(int id);
        Task<Book?> GetBookAsync(int bookId);
    }
}