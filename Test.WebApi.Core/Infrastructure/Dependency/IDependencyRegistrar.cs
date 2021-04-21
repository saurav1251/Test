using Autofac;
using Generic.Core.Infrastructure;
using Test.Data.Infrastructure.Dependency;
using Test.Services.Infrastructure.Dependency;
using DECLN_Core= Test.Core.Infrastructure.Dependency;
using Test.Core;
using Generic.Core.Infrastructure.DependencyManagement;

namespace Test.WebApi.Core.Infrastructure.Dependency
{
    /// <summary>
    /// Dependency registrar interface
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register Modules
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="appSettings">App settings</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterModule(new DECLN_Core.CoreModule());
            builder.RegisterModule(new CoreDbModule() { ConnectionID="" });
            builder.RegisterModule(new CoreServicesModule());
            RegisterDependencies(builder, typeFinder);
        }
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder"></param>
        private void RegisterDependencies(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<WebWorkContext>().As<IWorkContext>();

        }
        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0;
    }
}
