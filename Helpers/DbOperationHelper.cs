using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace restapi_crud_practice.Helpers
{
    public static class DbOperationHelper
    {
        public static async Task<int> ExecuteDeleteAsync<TEntity>(
            DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return await dbSet.Where(predicate).ExecuteDeleteAsync();
        }
        public static async Task<(bool Success, int RowsAffected)> ExecuteDeleteWithCountAsync<TEntity>(
            DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var rowsAffected = await ExecuteDeleteAsync(dbSet, predicate);
            return (rowsAffected > 0, rowsAffected);
        }
    }
}