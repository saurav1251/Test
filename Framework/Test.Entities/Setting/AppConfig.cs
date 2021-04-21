using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Test.Entities.Setting
{
    /// <summary>
    /// Represents the app settings
    /// </summary>
    public partial class AppConfig
    {
        #region Properties

        /// <summary>
        /// Gets or sets hosting configuration parameters
        /// </summary>
        public List<HostConfig> HostConfig { get; set; } = new List<HostConfig>();

        public CommonConfig CommonConfig { get { return CommonConfig.Instance; } }
        public string AppCode { get; set; }
        /// <summary>
        /// Gets or sets additional configuration parameters
        /// </summary>
        //[JsonExtensionData]
        //public IDictionary<string, JToken> AdditionalData { get; set; }

        #endregion
    }
}
