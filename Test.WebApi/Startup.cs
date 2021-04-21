using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Generic.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Test.WebApi.Core.Extensions;
using Generic.Core;
using Test.WebApi.Core.Infrastructure.Extensions;

namespace Test.WebApi
{
    public class Startup
    {
        #region Fields

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private IEngine _engine;

        #endregion
        #region Ctor

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //create default file provider
            CommonHelper.DefaultFileProvider = new GFileProvider(_webHostEnvironment);
            (_engine) = services.ConfigureApplicationServices(_configuration, _webHostEnvironment);
        }

        /// <summary>
        /// Configure the DI container 
        /// </summary>
        /// <param name="builder">Container builder</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            _engine.RegisterDependencies(builder);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            app.ConfigureRequestPipeline(env);
           
        }
    }
}
