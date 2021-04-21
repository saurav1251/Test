using Test.Core.Configuration;
using Test.Entities.Setting;
using Generic.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Test.WebApi.Core.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services, AppConfig appsetting)
        {
            try
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = String.Format("{0} API", appsetting.AppCode),
                    });
                    c.AddSecurityDefinition("Bearer",
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Please enter into field the word 'Bearer' following by space and JWT",
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey
                        });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void UseCustomSwagger(this IApplicationBuilder app, IConfigurationManager config)
        {
            try
            {
                var commConfig = config.AppSetting.CommonConfig;
                var hostConfig = config.AppSetting.HostConfig[0];
                string SubVDUri = string.IsNullOrEmpty(hostConfig.SubVD) ? string.Empty : "/" + hostConfig.SubVD.ToLower() + "/" + hostConfig.SubVD.ToLower() + "api";
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(SubVDUri + "/swagger/v1/swagger.json", config.AppSetting.AppCode + " V1");
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}