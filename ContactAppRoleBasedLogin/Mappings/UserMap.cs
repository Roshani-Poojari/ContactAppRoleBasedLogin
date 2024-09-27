using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppRoleBasedLogin.Models;
using FluentNHibernate.Mapping;

namespace ContactAppRoleBasedLogin.Mappings
{
    public class UserMap:ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(u => u.Id).GeneratedBy.GuidComb();
            Map(u => u.UserName);
            Map(u => u.Password);
            Map(u => u.FirstName);
            Map(u => u.LastName);
            Map(u => u.Email);
            Map(u => u.IsAdmin);
            Map(u => u.IsActive);
            HasOne(u=>u.Role).Cascade.All().PropertyRef(r=>r.User).Constrained();
            HasMany(u=>u.Contacts).Inverse().Cascade.All();
        }
    }
}