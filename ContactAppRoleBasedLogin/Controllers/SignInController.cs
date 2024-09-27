using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppRoleBasedLogin.Data;
using ContactAppRoleBasedLogin.Models;
using System.Web.Security;
using ContactAppRoleBasedLogin.ViewModels;

namespace ContactAppRoleBasedLogin.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var findUser = session.Query<User>().SingleOrDefault(u => u.UserName == loginVM.UserName);
                    if (findUser != null)
                    {
                        if (findUser.IsActive)
                        {
                            if (BCrypt.Net.BCrypt.Verify(loginVM.Password, findUser.Password))
                            {
                                FormsAuthentication.SetAuthCookie(loginVM.UserName, true);
                                Session["UserId"] = findUser.Id;
                                if (findUser.IsAdmin)
                                {
                                    return RedirectToAction("ViewAllStaff", "User");

                                }
                                else
                                {
                                    return RedirectToAction("Index", "Contact");
                                }
                            }
                        }
                    }
                    ModelState.AddModelError("", "UserName/Password doesn't exists");
                    return View();
                }
            }
        }

        [Authorize(Roles = "Admin,Staff")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}