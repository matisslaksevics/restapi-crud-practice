using restapi_crud_practice.Dtos.Borrow;
using restapi_crud_practice.Entities;

namespace restapi_crud_practice.Mapping
{
    public static class BorrowMapping
    {
        public static Borrow ToEntity(this CreateBorrowDto borrow)
        {
            return new Borrow()
            {
                ClientId = borrow.ClientId,
                BookId = borrow.BookId,
                BorrowDate = borrow.BorrowDate,
                ReturnDate = borrow.ReturnDate
            };
        }
        public static BorrowSummaryDto ToBorrowSummaryDto(this Borrow borrow)
        {
            return new(
                borrow.Id,
                borrow.Client!.FirstName,
                borrow.Book!.BookName,
                borrow.BorrowDate,
                borrow.ReturnDate
            );
        }
        public static BorrowDetailsDto ToBorrowDetailsDto(this Borrow borrow)
        {
            return new(
               borrow.Id,
               borrow.ClientId,
               borrow.BookId,
               borrow.BorrowDate,
               borrow.ReturnDate
           );
        }
        public static Borrow ToEntity(this UpdateBorrowDto borrow, int id)
        {
            return new Borrow()
            {
                Id = id,
                ClientId = borrow.ClientId,
                BookId = borrow.BookId,
                BorrowDate = borrow.BorrowDate,
                ReturnDate = borrow.ReturnDate
            };
        }
    }
}