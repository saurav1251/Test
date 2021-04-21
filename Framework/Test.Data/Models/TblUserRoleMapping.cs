using System;
using System.Collections.Generic;

#nullable disable

namespace Test.Data.Models
{
    public partial class TblUserRoleMapping : EntityBase
    {
        public long Id { get; set; }
        public int RoleId { get; set; }
        public long UserId { get; set; }

        public virtual TblRole Role { get; set; }
        public virtual TblUser User { get; set; }
    }
}
