using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services
{
    public interface IBorrowService
    {
        List<Borrow> GetAllBorrows();
    }
}