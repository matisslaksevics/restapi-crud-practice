using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Repositories.RBorrow;

namespace restapi_crud_practice.Services.SBorrow
{
    public class BorrowService(IBorrowRepository borrowRepository, BorrowHelper borrowHelper, ILogger<BorrowService> logger) : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository = borrowRepository;
        private readonly BorrowHelper _borrowHelper = borrowHelper;

        public async Task<List<BorrowSummaryDto>> GetAllBorrowsAsync()
        {
            logger.LogInformation("Service: GetAllBorrowsAsync requested.");
            try
            {
                var result = await _borrowRepository.GetAllBorrowsAsync();
                logger.LogInformation("Service: GetAllBorrowsAsync successful.");
                return result;

            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GetAllBorrowsAsync failed.");
                throw;
            }
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int id)
        {
            logger.LogInformation("Service: GetBorrowByIdAsync requested.");
            try
            {
                var result = await _borrowRepository.GetBorrowByIdAsync(id);

                if (result is not null)
                {
                    logger.LogInformation("Service: GetBorrowByIdAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Service: GetBorrowByIdAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GetBorrowByIdAsync failed.");
                throw;
            }
        }

        public async Task<List<BorrowSummaryDto>> GetAllClientBorrowsAsync(Guid? userId) 
        {
            logger.LogInformation("Service: GetAllClientBorrowsAsync requested.");
            try
            {
                var result = await _borrowRepository.GetAllClientBorrowsAsync(userId);
                logger.LogInformation("Service: GetAllClientBorrowsAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: GetAllClientBorrowsAsync failed.");
                throw;
            }
        }
        public async Task<BorrowSummaryDto> AdminCreateBorrowAsync(CreateBorrowDto newBorrow)
        {
            logger.LogInformation("Service: AdminCreateBorrowAsync requested.");
            try
            {
                var borrowEntity = new Borrow
                {
                    ClientId = newBorrow.ClientId,
                    BookId = newBorrow.BookId,
                    BorrowDate = newBorrow.BorrowDate,
                    ReturnDate = newBorrow.ReturnDate,
                    IsOverdue = _borrowHelper.CalculateIsOverdue(
                    newBorrow.BorrowDate, newBorrow.ReturnDate)
                };

                var createdBorrow = await _borrowRepository.CreateBorrowAsync(borrowEntity);

                if (createdBorrow is not null)
                {
                    logger.LogInformation("Service: AdminCreateBorrowAsync successful.");
                    return createdBorrow.ToBorrowSummaryDto();
                }
                else
                {
                    logger.LogError("Service: AdminCreateBorrowAsync failed.");
                    throw new InvalidOperationException("Failed to create borrow record");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: AdminCreateBorrowAsync failed.");
                throw;
            }
        }

        public async Task<BorrowSummaryDto> CreateBorrowAsync(CreateUserBorrowDto newBorrow, Guid? clientId)
        {
            logger.LogInformation("Service: CreateBorrowAsync requested.");
            try
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
                    IsOverdue = _borrowHelper.CalculateIsOverdue(
                        newBorrow.BorrowDate, newBorrow.ReturnDate)
                };

                var createdBorrow = await _borrowRepository.CreateBorrowAsync(borrowEntity);

                if (createdBorrow is not null)
                {
                    logger.LogInformation("Service: CreateBorrowAsync successful.");
                    return createdBorrow.ToBorrowSummaryDto();
                }
                else
                {
                    logger.LogError("Service: CreateBorrowAsync failed.");
                    throw new InvalidOperationException("Failed to create borrow record");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: CreateBorrowAsync failed.");
                throw;
            }
        }
        public async Task<bool> UpdateBorrowAsync(int id, UpdateBorrowDto updatedBorrow)
        {
            logger.LogInformation("Service: UpdateBorrowAsync requested.");
            try
            {
                var existingBorrow = await _borrowRepository.GetBorrowByIdAsync(id);
                if (existingBorrow == null)
                {
                    logger.LogError("Service: UpdateBorrowAsync failed.");
                    return false;
                }

                var updatedEntity = new Borrow
                {
                    Id = id,
                    BorrowDate = updatedBorrow.BorrowDate,
                    ReturnDate = updatedBorrow.ReturnDate,
                    ClientId = existingBorrow.ClientId,
                    BookId = existingBorrow.BookId,
                    IsOverdue = _borrowHelper.CalculateIsOverdue(
                    updatedBorrow.BorrowDate, updatedBorrow.ReturnDate)
                };

                var result = await _borrowRepository.UpdateBorrowAsync(id, updatedEntity);

                if (result)
                {
                    logger.LogInformation("Service: UpdateBorrowAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Service: UpdateBorrowAsync failed.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: UpdateBorrowAsync failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteBorrowAsync(int id)
        {
            logger.LogInformation("Service: DeleteBorrowAsync requested.");
            try
            {
                var result = await _borrowRepository.DeleteBorrowAsync(id);

                if (result is not (false, 0))
                {
                    logger.LogInformation("Service: DeleteBorrowAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Service: DeleteBorrowAsync failed.");
                    return (false, 0);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Service: DeleteBorrowAsync failed.");
                throw;
            }
        }
    }
}
