using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Test.Entities.Setting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Test.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureAppConfiguration((hostingContext,configuration) => {
                        string settingPath = hostingContext.HostingEnvironment.IsDevelopment() ? CommonConfig.Instance.DevAppSettingsFilePath : CommonConfig.Instance.ProdAppSettingsFilePath;
                        settingPath =Path.GetFullPath(settingPath, hostingContext.HostingEnvironment.ContentRootPath);
                        configuration.AddJsonFile(settingPath, true, true);
                    })
                    .UseStartup<Startup>();
                });
    }
}
