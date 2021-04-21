using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Test.Core.Utilities
{
    /// <summary>
    /// XML helper class
    /// </summary>
    public partial class JsonHelper
    {
        #region Methods


        /// <summary>
        /// Serialize objects to Json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T value)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return JsonSerializer.Serialize<T>(value, options);
        }

        /// <summary>
        /// DeSerialize Json to Object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
        #endregion
    }
}
