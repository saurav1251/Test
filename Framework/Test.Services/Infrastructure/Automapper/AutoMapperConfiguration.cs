#region Using

using AutoMapper;
using Generic.Core.Infrastructure.Mapper;
using Test.Data.Models;
using Test.Entities.User;

#endregion

namespace Test.Services.Infrastructure.AutoMapper
{
    public class ServiceMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 1;

        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Mapper configuration
        /// </summary>
        public MapperConfiguration MapperConfiguration { get; private set; }

        #endregion
        public ServiceMapperConfiguration()
        {
            CreateConfigMaps();

        }
        protected virtual void CreateConfigMaps()
        {

            CreateMap<AuthenticateResponse, TblUser>();
            CreateMap<TblUser, AuthenticateResponse>();


            CreateMap<Entities.User.User, TblUser>();
            CreateMap<TblUser, Entities.User.User>();

            CreateMap<Entities.User.UserRole, TblRole>();
            CreateMap<TblRole, Entities.User.UserRole>();

        }

        public void Init(MapperConfiguration config)
        {
            MapperConfiguration = config;
            Mapper = config.CreateMapper();
        }
    }
}