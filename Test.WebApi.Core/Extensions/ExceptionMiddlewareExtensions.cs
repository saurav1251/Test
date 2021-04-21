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

namespace Test.WebApi.Core.Infrastructure.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            var useDetailedExceptionPage = CommonConfig.Instance.DisplayFullErrorStack || webHostEnvironment.IsDevelopment();
            if (useDetailedExceptionPage)
            {
                //get detailed exceptions for developing and testing purposes
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //or use special exception handler
                app.UseExceptionHandler("/Error/Error");
            }
            //log errors
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    //log error
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                        return;
                    var logger = EngineContext.Current.Resolve<ILoggerService>();
                    try
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        //get current customer
                        var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;
                        Dictionary<string, string> paramError = new Dictionary<string, string>();

                        if (contextFeature != null)
                        {
                            try
                            {
                                if (contextFeature.Error.GetType() == typeof(OperationCanceledException))
                                {
                                    return;
                                }
                                if (currentUser != null)
                                {
                                    paramError.Add("UserID", currentUser.UserID.ToString());
                                    paramError.Add("UserName", currentUser.UserName);
                                    //paramError.Add("EmployeeID", currentUser.EmployeeID.ToString());
                                    //paramError.Add("EmailId", currentUser.Email);
                                }

                            }
                            catch (Exception)
                            { }
                            paramError.Add("ErrorKey", Guid.NewGuid().ToString());

                            logger.Error(contextFeature.Error.Message, contextFeature.Error);
                            await context.Response.WriteAsync(new
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = $"Internal Server Error.{new StringContent(paramError != null ? string.Concat(paramError.ToArray()) : "")}"
                            }.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message, ex);
                    }
                    finally
                    {
                        //rethrow the exception to show the error page
                        ExceptionDispatchInfo.Throw(exception);
                    }
                });
            });
        }
    }
}
