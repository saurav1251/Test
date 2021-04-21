using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Generic.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Test.Core;
using Test.Core.Configuration;
using Test.Core.Utilities;
using Test.Entities.Setting;
using Test.Entities.User;
using Test.Services.Authentication;
using Test.Services.User;
using Test.WebApi.Core.Controllers;

namespace Test.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfigurationManager _configurationManager;
        private readonly IJwtAuthManagerService _jwtAuthManagerService;
        private readonly ILoggerService _loggerService;
        private readonly IWorkContext _workContext;

        public UserController(IUserService userService,
            IConfigurationManager configurationManager,
            IJwtAuthManagerService jwtAuthManagerService,
            ILoggerService loggerService,
            IWorkContext workContext
            )
        {
            _userService = userService;
            _configurationManager = configurationManager;
            _jwtAuthManagerService = jwtAuthManagerService;
            _loggerService = loggerService;
            _workContext = workContext;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(Entities.User.AuthenticateResponse), (int)HttpStatusCode.OK)]
        public IActionResult Authenticate([FromBody] AuthenticateRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _userService.Authenticate(request);
            if (user == null)
            {
                return Unauthorized();
            }

            var role = _userService.GetUserRole(user.UserId);
            if (role != null)
                user.Role = role.ToList();
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.OriginalUserName),
                new Claim(ClaimTypes.UserData, JsonHelper.SerializeObject<AuthenticateResponse>( user))
            };
            foreach (var _role in user.Role)
            {
                claims.Append(new Claim(ClaimTypes.Role, _role.RoleName));
            }
            var jwtResult = _jwtAuthManagerService.GenerateTokens(user.UserName, claims, DateTime.Now);
            _loggerService.Debug($"User [{user.UserName}] logged in the system.");

            user.AccessToken = jwtResult.AccessToken;
            user.RefreshToken = jwtResult.RefreshToken;
            return Ok(user);
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Entities.User.User>), (int)HttpStatusCode.OK)]
        public IActionResult Get()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        [HttpGet("GetUserByToken")]
        [ProducesResponseType(typeof(Entities.User.User), (int)HttpStatusCode.OK)]
        public IActionResult GetUserByToken()
        {
            var users = _userService.GetById(_workContext.CurrentUser.UserID);
            return Ok(users);
        }

    }
}
