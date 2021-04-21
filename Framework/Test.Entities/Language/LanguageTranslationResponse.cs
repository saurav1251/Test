using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Entities
{
   public partial class LanguageTranslationResponse : EntityBaseModel
    {
        public string RESD_Type_ID { get; set; }
        public int RESD_LanguageID { get; set; }
        public int RESD_Code { get; set; }
        public string RESD_Value { get; set; }
        public string RESD_Comment { get; set; }
    }
}
