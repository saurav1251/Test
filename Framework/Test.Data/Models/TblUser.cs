using System;
using System.Collections.Generic;

#nullable disable

namespace Test.Data.Models
{
    public partial class TblUser : EntityBase
    {
        public TblUser()
        {
            TblUserRoleMapping = new HashSet<TblUserRoleMapping>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        public virtual ICollection<TblUserRoleMapping> TblUserRoleMapping { get; set; }
    }
}
