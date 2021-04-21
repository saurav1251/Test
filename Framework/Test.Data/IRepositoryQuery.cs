
#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Test.Data
{
    public interface IRepositoryQuery<TEntity> where TEntity : EntityBase
    {
        RepositoryQuery<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
        RepositoryQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        RepositoryQuery<TEntity> Include(Expression<Func<TEntity, object>> expression);
        IEnumerable<TEntity> GetPage(int page, int pageSize, out int totalCount);
        IQueryable<TEntity> Get();
        IQueryable<TEntity> GetWithNoTracking();
        IEnumerable<TEntity> GetPageWithNoTracking(int page, int pageSize, out int totalCount);
        Task<IEnumerable<TEntity>> GetAsync();
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
    }
}