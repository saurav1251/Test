using Test.Core.Infrastructure;
using Test.Entities.Setting;
using Generic.Core;
using Generic.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Test.Core.Configuration
{
    public partial class ConfigurationManager : IConfigurationManager
    {
        public string ServerID { get; }

        public AppConfig AppSetting { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConfigurationManager(AppConfig appSetting, IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                if (appSetting == null)
                    appSetting = new AppConfig();
                AppSetting = appSetting;
                _httpContextAccessor = httpContextAccessor;
                ServerID = _GetServerID();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private string _GetServerID()
        {
            try
            {
                string ServerID = string.Empty;
                if (AppSetting.HostConfig.Count==0)
                    ServerID = "0";
                else if (!string.IsNullOrEmpty(AppSetting.HostConfig[0].DBConnectionID) && string.IsNullOrEmpty(ServerID))
                {
                    ServerID = AppSetting.HostConfig[0].DBConnectionID;
                }
                else if (string.IsNullOrEmpty(ServerID))
                    ServerID = "0";
                return ServerID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
