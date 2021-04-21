using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Test.Data
{

    public interface IUnitOfWork: IUnitOfWorkForService
    {
        void CreateTransaction();
        void Commit();
        void Rollback();
        int Save();
    }
    // To be used in services e.g. ICustomerService, does not expose Save()
    // or the ability to Commit unit of work
    public interface IUnitOfWorkForService : IDisposable
    {
        IRepository<T> GenericRepository<T>() where T : EntityBase;
    }
}
