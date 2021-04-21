using System;
using System.Collections.Generic;
using System.Text;
using Test.Entities.Authentication;
using Test.Entities.User;

namespace Test.Entities
{
    public class CurrentUser : EntityBaseModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public List<UserRole> Role { get; set; }
        public CurrentUser()
        {
            Role = new List<UserRole>();
        }
    }
}
