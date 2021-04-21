#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Test.Data.Context
{
    public interface IDataContext : IDisposable
    {
        DbContextId ContextId { get; }
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        void SyncObjectState(object entity);
        DatabaseFacade Database { get; }
    }
}