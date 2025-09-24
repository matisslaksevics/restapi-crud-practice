using Microsoft.Extensions.Configuration;

namespace restapi_crud_practice.Helpers
{
    public class BorrowHelper
    {
        private readonly IConfiguration _configuration;

        public BorrowHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CalculateIsOverdue(DateOnly borrowDate, DateOnly? returnDate)
        {
            var maxMonths = _configuration.GetValue<int>("BorrowSettings:MaxBorrowMonths", 3);
            return returnDate is not null && borrowDate.AddMonths(maxMonths) < returnDate;
        }
    }
}