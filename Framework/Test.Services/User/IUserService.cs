using System;
using System.Collections.Generic;
using System.Text;
using Test.Entities.User;
using BE=Test.Entities.User;

namespace Test.Services.User
{
    public interface IUserService
    {
        BE.AuthenticateResponse Authenticate(BE.AuthenticateRequest model);
        IEnumerable<BE.User> GetAll();
        BE.User GetById(Int64 id);
        IEnumerable<UserRole> GetUserRole(Int64 UserId);
    }
}
