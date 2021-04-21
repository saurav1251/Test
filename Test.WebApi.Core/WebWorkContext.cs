using System;
using System.Linq;
using Test.Core;
using Test.Entities;
using Test.Services;
using Microsoft.AspNetCore.Http;

namespace Test.WebApi.Core
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Fields


        private readonly IAuthenticateService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CurrentUser _user;
        #endregion

        #region Ctor

        public WebWorkContext(
            IAuthenticateService authenticationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;

        }

        #endregion

        #region Utilities


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public virtual CurrentUser CurrentUser
        {
            get
            {
                //whether there is a cached value
                if (_user != null)
                    return _user;

                //try to get registered user
                _user = _authenticationService.GetAuthenticatedUser();

                return _user;
            }
            set
            {
                _user = value;
            }
        }
        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
