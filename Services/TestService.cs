using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Services
{
    public class TestService : IBorrowService
    {
        public List<Borrow> GetAllBorrows()
        {
            return new List<Borrow> { new Borrow { BookId = 1} };
        }
    }
}
