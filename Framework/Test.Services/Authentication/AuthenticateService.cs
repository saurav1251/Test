using Test.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using Test.Core.Utilities;

namespace Test.Services
{
    public class AuthenticateService:IAuthenticateService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticateService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Get authenticated User
        /// </summary>
        /// <returns>Customer</returns>
        public virtual CurrentUser GetAuthenticatedUser()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                var claimes = user.Claims.ToList();
                if (user.Identity.IsAuthenticated)
                {
                    

                    var profileData = claimes.Where(P => P.Type == ClaimTypes.UserData).SingleOrDefault().Value;
                    var dynamicObjUser = JsonHelper.DeSerializeObject<CurrentUser>(profileData);
                   
                    return dynamicObjUser;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
