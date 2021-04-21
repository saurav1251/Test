using Autofac;
using Test.Core.Configuration;
using Generic.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using GE_DEP = Generic.Core.Infrastructure.DependencyManagement;
namespace Test.Core.Infrastructure.Dependency
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new GE_DEP.CoreModule());
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>().InstancePerLifetimeScope();
        }
    }
}
