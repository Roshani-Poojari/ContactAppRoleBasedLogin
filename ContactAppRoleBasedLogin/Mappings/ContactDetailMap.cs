using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppRoleBasedLogin.Models;
using FluentNHibernate.Mapping;

namespace ContactAppRoleBasedLogin.Mappings
{
    public class ContactDetailMap:ClassMap<ContactDetail>
    {
        public ContactDetailMap()
        {
            Table("ContactDetails");
            Id(cd => cd.Id).GeneratedBy.GuidComb();
            Map(cd => cd.Number);
            Map(cd => cd.Email);
            References(cd => cd.Contact).Column("ContactId").Cascade.None();
        }
    }
}