using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace Test.Data.Models
{
    public partial class TblRole : EntityBase
    {
        public TblRole()
        {
            TblUserRoleMapping = new HashSet<TblUserRoleMapping>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public BitArray Active { get; set; }

        public virtual ICollection<TblUserRoleMapping> TblUserRoleMapping { get; set; }
    }
}
