using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactAppRoleBasedLogin.Models
{
    public class Role
    {
        public virtual Guid Id { get; set; }
        public virtual string RoleName { get; set; }
        public virtual User User { get; set; }
    }
}