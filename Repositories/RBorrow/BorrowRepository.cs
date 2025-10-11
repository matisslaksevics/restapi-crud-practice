using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Mapping;

namespace restapi_crud_practice.Repositories.RBorrow
{
    public class BorrowRepository(BookBorrowingContext dbContext, ILogger<BorrowRepository> logger) : IBorrowRepository
    {
        private readonly BookBorrowingContext dbContext = dbContext;

        public async Task<List<BorrowSummaryDto>> GetAllBorrowsAsync()
        {
            logger.LogInformation("Repository: GetAllBorrowsAsync requested.");
            try
            {
                var result = await dbContext.Borrows.Where(borrow => borrow.IsOverdue == true)
                .Include(borrow => borrow.Client)
                .Include(borrow => borrow.Book)
                .Select(borrow => borrow
                .ToBorrowSummaryDto())
                .ToListAsync();

                logger.LogInformation("Repository: GetAllBorrowsAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetAllBorrowsAsync failed.");
                throw;
            }
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int id)
        {
            logger.LogInformation("Repository: GetBorrowByIdAsync requested.");
            try
            {
                var result = await dbContext.Borrows
                .Include(b => b.Client)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == id);

                if (result is not null)
                {
                    logger.LogInformation("Repository: GetBorrowByIdAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Repository: GetBorrowByIdAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetBorrowByIdAsync failed.");
                throw;
            }
        }
        public async Task<List<BorrowSummaryDto>> GetAllClientBorrowsAsync(Guid? userId)
        {
            logger.LogInformation("Repository: GetAllClientBorrowsAsync requested.");
            try
            {
                var result = await dbContext.Borrows.Where(borrow => borrow.IsOverdue == true)
                .Where(borrow => borrow.ClientId == userId)
                .Include(borrow => borrow.Client)
                .Include(borrow => borrow.Book)
                .Select(borrow => borrow
                .ToBorrowSummaryDto())
                .ToListAsync();

                logger.LogInformation("Repository: GetAllClientBorrowsAsync successful.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetAllClientBorrowsAsync failed.");
                throw;
            }
        }

        public async Task<Borrow?> CreateBorrowAsync(Borrow borrow)
        {
            logger.LogInformation("Repository: CreateBorrowAsync requested.");
            try
            {
                dbContext.Borrows.Add(borrow);
                await dbContext.SaveChangesAsync();
                var result = await dbContext.Borrows
                .Include(b => b.Client)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == borrow.Id);

                if (result is not null)
                {
                    logger.LogInformation("Repository: CreateBorrowAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Repository: CreateBorrowAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: CreateBorrowAsync failed.");
                throw;
            }
        }
        public async Task<Client?> GetClientAsync(Guid? clientId)
        {
            logger.LogInformation("Repository: GetClientAsync requested.");
            try
            {
                if (clientId == null)
                {
                    return null;
                }
                var result = await dbContext.Clients.FindAsync(clientId);

                if (result is not null)
                {
                    logger.LogInformation("Repository: GetClientAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Repository: GetClientAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetClientAsync failed.");
                throw;
            }
        }

        public async Task<Book?> GetBookAsync(int bookId)
        {
            logger.LogInformation("Repository: GetBookAsync requested.");
            try
            {
                var result = await dbContext.Books.FindAsync(bookId);

                if (result is not null)
                {
                    logger.LogInformation("Repository: GetBookAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Repository: GetBookAsync failed.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: GetBookAsync failed.");
                throw;
            }
        }

        public async Task<bool> UpdateBorrowAsync(int id, Borrow borrow)
        {
            logger.LogInformation("Repository: UpdateBorrowAsync requested.");
            try
            {
                var existingBorrow = await dbContext.Borrows.FindAsync(id);
                if (existingBorrow is null)
                {
                    return false;
                }

                dbContext.Entry(existingBorrow).CurrentValues.SetValues(borrow);
                existingBorrow.ClientId = borrow.ClientId;
                existingBorrow.BookId = borrow.BookId;

                var result = await dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    logger.LogInformation("Repository: UpdateBorrowAsync successful.");
                    return true;
                }
                else
                {
                    logger.LogError("Repository: UpdateBorrowAsync failed.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: UpdateBorrowAsync failed.");
                throw;
            }
        }
        public async Task<(bool Success, int RowsAffected)> DeleteBorrowAsync(int id)
        {
            logger.LogInformation("Repository: DeleteBorrowAsync requested.");
            try
            {
                var result = await DbOperationHelper.ExecuteDeleteWithCountAsync(dbContext.Borrows, borrow => borrow.Id == id);

                if (result is not (false, 0))
                {
                    logger.LogInformation("Repository: DeleteBorrowAsync successful.");
                    return result;
                }
                else
                {
                    logger.LogError("Repository: DeleteBorrowAsync failed.");
                    return (false, 0);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Repository: DeleteBorrowAsync failed.");
                throw;
            }
        }
    }
}
