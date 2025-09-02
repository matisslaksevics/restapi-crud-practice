using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace restapi_crud_practice.Helpers
{
    public static class DbOperationHelper
    {
        public static async Task<bool> ExecuteDeleteAsync<TEntity>(
        DbSet<TEntity> dbSet,
        Expression<Func<TEntity, bool>> predicate)
        where TEntity : class
        {
            var rowsAffected = await dbSet.Where(predicate).ExecuteDeleteAsync();
            return rowsAffected > 0;
        }
    }
}
