using Test.Entities.Setting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Configuration
{
    public interface IConfigurationManager
    {
        AppConfig AppSetting { get; }
        string ServerID { get; }

    }
}
