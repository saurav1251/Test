namespace Test.Entities.Setting
{
    /// <summary>
    /// Represents common configuration parameters
    /// </summary>
    public class CommonConfig
    {
        #region Singleton implementation

        // Make constructor private to avoid instantiation from the outside
        private CommonConfig()
        {
        }

        // Create unique instance
        private static readonly CommonConfig _instance = new CommonConfig();

        // Expose unique instance
        public static CommonConfig Instance
        {
            get { return _instance; }
        }

        #endregion

        // instance properties
        // ...
        /// <summary>
        /// Gets or sets a value indicating whether to display the full error in production environment. It's ignored (always enabled) in development environment
        /// </summary>
        public bool DisplayFullErrorStack { get { return false; } }
        public string ApplicationSecret { get { return "dHRpY2ExGjAYBgNVBAsMEUx1eG9"; } } 
        public string DevAppSettingsFilePath { get { return "../Test.ClientApp/src/assets/config.json"; } } 
        public string ProdAppSettingsFilePath { get { return "../UI/assets/config.json"; } } 


    }
}