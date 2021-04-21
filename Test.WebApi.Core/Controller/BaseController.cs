using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Test.WebApi.Core.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        #region Notifications

        /// <summary>
        /// PreconditionFailed's JSON data
        /// </summary>
        /// <param name="error">Error text</param>
        /// <returns>Error's JSON data</returns>
        protected IActionResult PreconditionFailed(string error)
        {
            return StatusCode(412, new
            {
                Description = "Precondition Failed",
                Message = error
            });
        }

        /// <summary>
        /// PreconditionFailed's JSON data
        /// </summary>
        /// <param name="errors">Error messages</param>
        /// <returns>Error's JSON data</returns>
        protected IActionResult PreconditionFailed(object errors)
        {
            return StatusCode(412, new
            {
                Description = "Precondition Failed",
                Message = errors
            });
        }
        /// <summary>
        /// ExpectationFailed's JSON data
        /// </summary>
        /// <param name="error">Error text</param>
        /// <returns>Error's JSON data</returns>
        protected IActionResult ExpectationFailed(string error)
        {
            return StatusCode(412, new
            {
                Description = "Expectation Failed",
                Message = error
            });
        }

        /// <summary>
        /// ExpectationFailed's JSON data
        /// </summary>
        /// <param name="errors">Error messages</param>
        /// <returns>Error's JSON data</returns>
        protected IActionResult ExpectationFailed(object errors)
        {
            return StatusCode(412, new
            {
                Description = "Expectation Failed",
                Message = errors
            });
        }
        /// <summary>
        /// Bad Request's JSON data
        /// </summary>
        /// <param name="error">Error text</param>
        /// <returns>Error's JSON data</returns>
        protected IActionResult BadRequest(string error)
        {
            return StatusCode(400, new
            {
                Description = "Bad Request",
                Message = error
            });
        }

        
        #endregion

    }
}