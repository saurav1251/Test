using System;
using System.Linq;
using System.Net;
using Test.Core.Configuration;
using Test.Data.Context;
using Test.Entities.Setting;
using FluentValidation.AspNetCore;
using Generic.Core.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using DECLN_INF = Test.Core.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using Test.WebApi.Core.Infrastructure.Extensions;

namespace Test.WebApi.Core.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <param name="webHostEnvironment">Hosting environment</param>
        /// <returns>Configured engine and app settings</returns>
        public static IEngine ConfigureApplicationServices(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            //adding application setting 
            var appsetting = services.AddApplicationSetting(configuration);

            //create engine and configure service provider
            var engine = EngineContext.WCreate();

            //adding core services {automapper, context accessor}
            engine.ConfigureServices(services, configuration);

            //adding authorization policy to prevent un-authorized user
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizedPolicy",
                    policyBuilder => policyBuilder.RequireAuthenticatedUser());
            });

            //adding webapi controller with authorization required
            var mvcBuilder = services.AddControllers(options => options.Filters.Add(new AuthorizeFilter("AuthorizedPolicy")));

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            mvcBuilder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            //add fluent validation
            mvcBuilder.AddFluentValidation(configuration =>
            {
                //register all available validators from Application assemblies
                var assemblies = mvcBuilder.PartManager.ApplicationParts
                    .OfType<AssemblyPart>()
                    .Where(part => part.Name.StartsWith(appsetting.AppCode, StringComparison.InvariantCultureIgnoreCase))
                    .Select(part => part.Assembly);
                configuration.RegisterValidatorsFromAssemblies(assemblies);

                //implicit/automatic validation of child properties
                configuration.ImplicitlyValidateChildProperties = true;
            });

            //register controllers as services, it'll allow to override them
            mvcBuilder.AddControllersAsServices();

            //adding options
            services.AddOptions();

            services.AddRouting();

            //add swagger extensions
            services.AddSwagger(appsetting);

            // adding identity server services
            services.AddIdentityServer(configuration, appsetting);


            return engine;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        private static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration, AppConfig appsetting)
        {
            //adding jwt authentication services
            services.ConfigureTokenAuthentication(appsetting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        private static AppConfig AddApplicationSetting(this IServiceCollection services, IConfiguration configuration)
        {

            //add configuration parameters
            var appSettings = new AppConfig();
            configuration.Bind(appSettings);
            string currentHostEnv = Convert.ToString(Environment.GetEnvironmentVariable("HRIS_SiteName")).ToLower().Trim();

            //removing all hosting except this host
            appSettings.HostConfig.RemoveAll(x => !x.Host.ToLower().Trim().Split("$#$", StringSplitOptions.None).Contains(currentHostEnv.ToLower().Trim()));
            services.AddSingleton(appSettings);
            return appSettings;
        }

    }
}