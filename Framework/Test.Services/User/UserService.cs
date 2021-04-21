using System;
using System.Collections.Generic;
using System.Text;
using Test.Data;
using Test.Data.Models;
using Test.Entities.User;
using System.Linq;
using Test.Services.Extensions;
using Test.Data.SqlHelper;

namespace Test.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRepository<TblUser> _repositoryUser;
        private readonly IRepository<TblUserRoleMapping> _repositoryUserRoleMapping;
        private readonly IRepository<TblRole> _repositoryRole;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoredProcBuilder _storedProcBuilder;

        public UserService(IRepository<TblUser> repositoryUser,
            IRepository<TblUserRoleMapping> repositoryUserRoleMapping,
            IRepository<TblRole> repositoryRole,
            IStoredProcBuilder storedProcBuilder,
            IUnitOfWork unitOfWork)
        {
            _repositoryUser = repositoryUser;
            _unitOfWork = unitOfWork;
            _repositoryRole = repositoryRole;
            _repositoryUserRoleMapping = repositoryUserRoleMapping;
            _storedProcBuilder = storedProcBuilder;

        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            try
            {
                List<dynamic> resuslt = null;
                _storedProcBuilder.StoredProc("Test.fn_GetUserWithRole")
                    .AddParam("puser_name","test")
                    .Exec(x => resuslt = x.ToList<dynamic>());

                var user = _repositoryUser.Query()
               .Filter(x => x.UserName == model.UserName && x.Password == model.Password)
               .GetWithNoTracking().FirstOrDefault();
                var UserModel = user.ToModel<AuthenticateResponse>();
                UserModel.UserId = user.Id;
                UserModel.OriginalUserName = user.FirstName + " " + user.LastName;

                return UserModel;
            }
            catch (ArgumentNullException)
            {
                return null;
            }

        }
        public IEnumerable<UserRole> GetUserRole(Int64 UserId)
        {
            try
            {
                var userRole = _repositoryUserRoleMapping.Query()
                                .Filter(x => x.UserId == UserId)
                                .Include(x => x.Role)
                                .GetWithNoTracking().Select(x => x.Role.ToModel<UserRole>());

                return userRole;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
        public IEnumerable<Entities.User.User> GetAll()
        {

            var user = _repositoryUser.Query()
                .GetWithNoTracking().Select(x => x.ToModel<Entities.User.User>());
            return user;
        }

        public Entities.User.User GetById(Int64 id)
        {
            var user = _repositoryUser.Query()
               .Filter(x => x.Id == id)
               .GetWithNoTracking().FirstOrDefault();
            return user.ToModel<Entities.User.User>();
        }
    }
}
