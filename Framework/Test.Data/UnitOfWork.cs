using Test.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Test.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
         //Here TContext is nothing but your DBContext class
        //In our example it is EmployeeDBContext class
        private readonly IDataContext _context;
        private bool _disposed;
        private string _errorMessage = string.Empty;
        private IDbContextTransaction _objTran;
        private Dictionary<string, object> _repositories;
        //Using the Constructor we are initializing the _context variable is nothing but
        //we are storing the DBContext (EmployeeDBContext) object in _context variable
        public UnitOfWork(IDataContext dbContext)
        {
            _context = dbContext;
        }
        //The Dispose() method is used to free unmanaged resources like files, 
        //database connections etc. at any time.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //This Context property will return the DBContext object i.e. (EmployeeDBContext) object
        
        //This CreateTransaction() method will create a database Trnasaction so that we can do database operations by
        //applying do evrything and do nothing principle
        public void CreateTransaction()
        {
            _objTran = _context.Database.BeginTransaction();
        }
        //If all the Transactions are completed successfuly then we need to call this Commit() 
        //method to Save the changes permanently in the database
        public void Commit()
        {
           // _context.Database.CommitTransaction();
            _objTran.Commit();

        }
        //If atleast one of the Transaction is Failed then we need to call this Rollback() 
        //method to Rollback the database changes to its previous state
        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }
        //This Save() Method Implement DbContext Class SaveChanges method so whenever we do a transaction we need to
        //call this Save() method so that it will make the changes in the database
        public int Save()
        {
            try
            {
               return _context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                throw dbEx;
                //foreach (var validationErrors in dbEx.Data)
                //    foreach (var validationError in validationErrors)
                //        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                //throw new Exception(_errorMessage, dbEx);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        public IRepository<T> GenericRepository<T>() where T : EntityBase
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IRepository<T>)_repositories[type];
        }
    }
}
