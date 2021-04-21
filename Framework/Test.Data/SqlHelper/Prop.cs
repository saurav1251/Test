using System;

namespace Test.Data.SqlHelper
{
    struct Prop
    {
        public int ColumnOrdinal { get; set; }
        public Action<object, object> Setter { get; set; }
    }
}
