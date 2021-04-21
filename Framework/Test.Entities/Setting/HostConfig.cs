using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Entities.Setting
{
    public class HostConfig
    {
        public string Host { get; set; }
        public string SubVD { get; set; }
        public List<string> ApiUrl { get; set; } = new List<string>();
        public string DBConnectionID { get; set; }
        public string DBServerFile { get; set; } = "hris_ecdm";
        
    }
}
