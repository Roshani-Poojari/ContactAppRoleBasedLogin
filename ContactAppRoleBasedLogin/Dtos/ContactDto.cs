using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactAppRoleBasedLogin.Dtos
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
    }
}