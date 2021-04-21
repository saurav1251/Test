using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Test.Core.Infrastructure;
using Generic.Core.Services;
using Generic.Core.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Test.Core.Configuration;
using System.Runtime.ExceptionServices;
using Test.Core;
using Test.Entities.Setting;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Test.WebApi.Core.Infrastructure.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection ConfigureTokenAuthentication(this IServiceCollection services, AppConfig appsetting)
        {
            var commConfig = appsetting.CommonConfig;
            var hostConfig = appsetting.HostConfig[0];
            string hostName = hostConfig.Host.Contains("localhost") ? "localhost" : hostConfig.Host;

            var key = Encoding.ASCII.GetBytes(appsetting.CommonConfig.ApplicationSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = hostName,
                    ValidAudience = hostName,

                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        //var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        //var userId = int.Parse(context.Principal.Identity.Name);
                        //var user = userService.GetById(userId);
                        //if (user == null)
                        //{
                        //    // return unauthorized if user no longer exists
                        //    context.Fail("Unauthorized");
                        //}
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
