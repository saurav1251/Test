using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data.Context
{
    public interface IDBContextFactory
    {
        IDataContext GetInstance();
        string ConnectionString { get; }
    }
}
