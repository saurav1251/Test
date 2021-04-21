using Test.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Services
{
    public interface IAuthenticateService
    {
        CurrentUser GetAuthenticatedUser();
    }
}
