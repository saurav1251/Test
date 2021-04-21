using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Entities.User
{
    public class UserRole:EntityBaseModel
    {
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}
