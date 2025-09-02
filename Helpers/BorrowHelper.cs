namespace restapi_crud_practice.Helpers
{
    public static class BorrowHelper
    {
        public static bool CalculateIsOverdue(DateOnly borrowDate, DateOnly? returnDate)
        {
            return returnDate is not null && borrowDate.AddMonths(3) < returnDate;
        }
    }
}
