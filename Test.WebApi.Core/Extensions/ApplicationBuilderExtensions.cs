using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Test.Core.Infrastructure;
using Test.WebApi.Core.Extensions;
using Generic.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Test.Core.Configuration;

namespace Test.WebApi.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void ConfigureRequestPipeline(this IApplicationBuilder application, IWebHostEnvironment env)
        {
            // Adding Defautl Request pipeline eg. Cors, Authentication, Forwarded Header, HSTS, Https Redirection, 
            EngineContext.Current.ConfigureRequestPipeline(application);

            //adding pipeline services
            application.StartEngine();
        }

        public static void StartEngine(this IApplicationBuilder application)
        {
            var config = EngineContext.Current.Resolve<IConfigurationManager>();
            var webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();


            // for routing pipeline
            application.UseRouting();

            //for authorization services
            application.UseAuthorization();

            //for deafult endpoints
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Using Standard Code pages
            application.UseStatusCodePages();

            //configuring exception middleware
            application.ConfigureExceptionHandler(webHostEnvironment);

            //registering swagger
            application.UseCustomSwagger(config);

        }
    }
}
