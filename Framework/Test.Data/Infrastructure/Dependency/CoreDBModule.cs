using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Data.SqlHelper;
using Test.Data.Context;
namespace Test.Data.Infrastructure.Dependency
{
    /// <summary>
    /// DB Related registrations
    /// </summary>
    public class CoreDbModule : Module
    {
        public string ConnectionID { get; set; } = string.Empty;
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DBContextFactory>().As<IDBContextFactory>().WithParameter("ConnectionID", ConnectionID).InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<IDBContextFactory>().GetInstance()).As<IDataContext>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<StoredProcBuilder>().As<IStoredProcBuilder>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}
