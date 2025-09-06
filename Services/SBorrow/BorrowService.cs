using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Repositories.RBorrow;

namespace restapi_crud_practice.Services.SBorrow
{
    public class BorrowService(IBorrowRepository borrowRepository) : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository = borrowRepository;

        public async Task<List<BorrowSummaryDto>> GetAllBorrowsAsync()
        {
            return await _borrowRepository.GetAllBorrowsAsync();
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int id)
        {
            return await _borrowRepository.GetBorrowByIdAsync(id);
        }

        public async Task<List<BorrowSummaryDto>> GetAllClientBorrowsAsync(Guid? userId) 
        {
            return await _borrowRepository.GetAllClientBorrowsAsync(userId);
        }
        public async Task<BorrowSummaryDto> AdminCreateBorrowAsync(CreateBorrowDto newBorrow)
        {
            var borrowEntity = new Borrow
            {
                ClientId = newBorrow.ClientId,
                BookId = newBorrow.BookId,
                BorrowDate = newBorrow.BorrowDate,
                ReturnDate = newBorrow.ReturnDate,
                IsOverdue = BorrowHelper.CalculateIsOverdue(
                    newBorrow.BorrowDate, newBorrow.ReturnDate)
            };

            var createdBorrow = await _borrowRepository.CreateBorrowAsync(borrowEntity);

            if (createdBorrow == null)
            {
                throw new InvalidOperationException("Failed to create borrow record");
            }
            return createdBorrow.ToBorrowSummaryDto();
        }

        public async Task<BorrowSummaryDto> CreateBorrowAsync(CreateUserBorrowDto newBorrow, Guid? clientId)
        {
            if (clientId == null)
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            var borrowEntity = new Borrow
            {
                ClientId = clientId.Value,
                BookId = newBorrow.BookId,
                BorrowDate = newBorrow.BorrowDate,
                ReturnDate = newBorrow.ReturnDate,
                IsOverdue = BorrowHelper.CalculateIsOverdue(
                    newBorrow.BorrowDate, newBorrow.ReturnDate)
            };

            var createdBorrow = await _borrowRepository.CreateBorrowAsync(borrowEntity);
            if (createdBorrow == null)
            {
                throw new InvalidOperationException("Failed to create borrow record");
            }
            return createdBorrow.ToBorrowSummaryDto();
        }
        public async Task<bool> UpdateBorrowAsync(int id, UpdateBorrowDto updatedBorrow)
        {
            var existingBorrow = await _borrowRepository.GetBorrowByIdAsync(id);
            if (existingBorrow == null)
            {
                return false;
            } 

            var updatedEntity = new Borrow
            {
                Id = id,
                BorrowDate = updatedBorrow.BorrowDate,
                ReturnDate = updatedBorrow.ReturnDate,
                ClientId = existingBorrow.ClientId, 
                BookId = existingBorrow.BookId, 
                IsOverdue = BorrowHelper.CalculateIsOverdue(
                    updatedBorrow.BorrowDate, updatedBorrow.ReturnDate)
            };

            return await _borrowRepository.UpdateBorrowAsync(id, updatedEntity);
        }
        public async Task<bool> DeleteBorrowAsync(int id)
        {
            return await _borrowRepository.DeleteBorrowAsync(id);
        }
    }
}
