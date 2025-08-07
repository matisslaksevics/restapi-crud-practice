using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Mapping;
using restapi_crud_practice.Dtos.Borrow;
namespace restapi_crud_practice.Services
{
    public class BorrowService
    {
        private readonly BookBorrowingContext dbContext;
        public BorrowService(BookBorrowingContext dbContext)
        {
            this.dbContext = dbContext;
        }
        


    }
}
