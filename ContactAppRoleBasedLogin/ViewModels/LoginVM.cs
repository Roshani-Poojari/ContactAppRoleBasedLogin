using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContactAppRoleBasedLogin.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please enter username!!")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password!!")]
        public string Password { get; set; }
    }
}