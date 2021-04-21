using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Entities.User
{
    public class AuthenticateRequest:EntityBaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
