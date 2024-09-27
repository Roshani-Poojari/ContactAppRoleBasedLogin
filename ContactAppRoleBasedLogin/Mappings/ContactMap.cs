using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppRoleBasedLogin.Models;
using FluentNHibernate.Mapping;

namespace ContactAppRoleBasedLogin.Mappings
{
    public class ContactMap:ClassMap<Contact>
    {
        public ContactMap()
        {
            Table("Contacts");
            Id(c => c.Id).GeneratedBy.GuidComb();
            Map(c => c.FirstName);
            Map(c => c.LastName);
            Map(c => c.IsActive);
            References(c => c.User).Column("UserId").Cascade.None();
            HasMany(c => c.ContactDetails).Inverse().Cascade.All();
        }
    }
}