using Autofac;
using Test.Services.Authentication;
using Test.Services.User;

namespace Test.Services.Infrastructure.Dependency
{
    /// <summary>
    /// Dependency registrar interface
    /// </summary>
    public class CoreServicesModule : Module
    {
        /// <summary>
        /// Service Related registrations
        /// </summary>

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticateService>().As<IAuthenticateService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<JwtAuthManagerService>().As<IJwtAuthManagerService>().InstancePerLifetimeScope();
           
        }
    }
}
