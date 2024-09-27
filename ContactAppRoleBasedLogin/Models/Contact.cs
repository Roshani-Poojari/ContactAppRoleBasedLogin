using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAppRoleBasedLogin.Models
{
    public class Contact
    {
        public virtual Guid Id { get; set; }

        [Required(ErrorMessage = "Please enter first name!!")]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name!!")]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [Display(Name = "Active")]
        public virtual bool IsActive { get; set; }
        public virtual User User { get; set; }
        public virtual IList<ContactDetail> ContactDetails { get; set; } = new List<ContactDetail>();

    }
}