using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAppRoleBasedLogin.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }

        [Required(ErrorMessage = "Please enter username!!")]
        [Display(Name = "Username")]
        public virtual string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password!!")]
        public virtual string Password { get; set; }

        [Required(ErrorMessage = "Please enter first name!!")]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name!!")]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Id")]
        public virtual string Email { get; set; }

        public virtual bool IsAdmin { get; set; }

        [Display(Name = "Active")]
        public virtual bool IsActive { get; set; }
        public virtual IList<Contact> Contacts { get; set; } = new List<Contact>();
        public virtual Role Role { get; set; } = new Role();
    }
}