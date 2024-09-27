using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppRoleBasedLogin.Data;
using System.Web.Security;
using ContactAppRoleBasedLogin.Models;
using NHibernate.Linq;

namespace ContactAppRoleBasedLogin.Controllers
{
    /*
     ALTER TABLE Users
ADD CONSTRAINT DF_Users_Id DEFAULT NEWID() FOR Id
ALTER TABLE Contacts
ADD CONSTRAINT DF_Contacts_Id DEFAULT NEWID() FOR Id
ALTER TABLE ContactDetails
ADD CONSTRAINT DF_ContactDetails_Id DEFAULT NEWID() FOR Id
ALTER TABLE Roles
ADD CONSTRAINT DF_Roles_Id DEFAULT NEWID() FOR Id*/
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        public ActionResult ViewAllStaff()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var staffs = session.Query<User>().Where(u => u.IsAdmin == false).ToList();
                return View(staffs);
            }
        }
        public ActionResult ViewStaffContacts(Guid userId)
        {
            using(var session = NHibernateHelper.CreateSession())
            {
                var contacts = session.Query<Contact>().Where(c=>c.User.Id == userId).ToList();
                return View(contacts);
            }
        }
        public ActionResult ViewStaffContactDetails(Guid contactId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var contactDetails = session.Query<ContactDetail>().Where(cd=>cd.Contact.Id == contactId).ToList();
                return View(contactDetails);
            }
        }
        public ActionResult ViewAllAdmins()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var admins = session.Query<User>().Where(u => u.IsAdmin == true).ToList();
                return View(admins);
            }
        }
        public ActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            user.Password= BCrypt.Net.BCrypt.HashPassword(user.Password);
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    user.Role.User = user;
                    user.IsActive = true;
                    if (user.IsAdmin)
                    {
                        user.Role.RoleName = "Admin";
                    }
                    else
                    {
                        user.Role.RoleName = "Staff";
                    }
                    session.Save(user);
                    transaction.Commit();
                    return RedirectToAction("ViewAllStaff");
                }
            }
        }
        public ActionResult EditUser(Guid userId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var findUser = session.Get<User>(userId);
                return View(findUser);
            }
        }
        [HttpPost]

        public ActionResult EditUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            using (var session = NHibernateHelper.CreateSession())
            {
                using(var transaction = session.BeginTransaction())
                {
                    user.Role.User = user;
                    session.Update(user);
                    transaction.Commit();
                    if (user.IsAdmin)
                    {
                        return RedirectToAction("ViewAllAdmins");
                    }
                    return RedirectToAction("ViewAllStaff");
                }
            }
        }
        [HttpPost]
        public ActionResult ToggleIsActive(Guid userId, bool isActive)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);
                    if (user != null)
                    {
                        user.IsActive = isActive;
                        session.Update(user);
                        transaction.Commit();
                    }
                }
            }

            return Json(new { success = true });
        }

    }
}