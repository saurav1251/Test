using System;
using System.Collections.Generic;
using System.Text;
using Test.Entities.Authentication;

namespace Test.Entities.User
{
    public class AuthenticateResponse : EntityBaseModel
    {
        public Int64 UserId { get; set; }
        public string UserName { get; set; }
        public List<UserRole> Role { get; set; }
        public string OriginalUserName { get; set; }
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public AuthenticateResponse()
        {
            Role = new List<UserRole>();
        }
    }
}
