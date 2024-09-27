using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppRoleBasedLogin.Data;
using ContactAppRoleBasedLogin.Models;
using ContactAppRoleBasedLogin.Dtos;
using NHibernate.Linq;

namespace ContactAppRoleBasedLogin.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewContacts()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            Guid userId = (Guid)Session["UserId"];
            using (var session = NHibernateHelper.CreateSession())
            {
                var userWithContacts = session.Query<User>().FetchMany(u => u.Contacts).SingleOrDefault(u => u.Id == userId);

                if (userWithContacts != null && userWithContacts.Contacts.Count > 0)
                {
                    var contactsDto = userWithContacts.Contacts.Select(contact => new ContactDto
                    {
                        Id = contact.Id,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        IsActive = contact.IsActive
                    }).ToList();

                    return Json(contactsDto, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public ActionResult CreateContact(Contact contact)
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(401, "Unauthorized");
            }
            Guid userId = (Guid)Session["UserId"];
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Fetch the user from the database
                        var user = session.Query<User>().SingleOrDefault(u => u.Id == userId);

                        if (user == null)
                        {
                            return HttpNotFound("User not found");
                        }

                        contact.IsActive = true;

                        // Associate the new contact with the logged-in user
                        contact.User = user;

                        // Save the contact to the database
                        session.Save(contact);
                        transaction.Commit();

                        return Json(new
                        {
                            Id = contact.Id,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            IsActive = contact.IsActive
                        });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(500, "Error adding contact: " + ex.Message);
                    }
                }
            }
        }
        public ActionResult GetContactById(Guid contactId)
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(401, "Unauthorized");
            }

            using (var session = NHibernateHelper.CreateSession())
            {
                var contact = session.Get<Contact>(contactId);
                if (contact == null)
                {
                    return Json(new { success = false, message = "Contact not found" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    success = true,
                    contact = new
                    {
                        contact.Id,
                        contact.FirstName,
                        contact.LastName
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }
        /*
         if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }*/
        [HttpPost]
        public ActionResult EditContact(Contact contact)
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(401, "Unauthorized");
            }
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var existingContact = session.Get<Contact>(contact.Id);
                        if (existingContact == null)
                        {
                            return Json(new { success = false, message = "Contact not found" });
                        }

                        existingContact.FirstName = contact.FirstName;
                        existingContact.LastName = contact.LastName;

                        session.Update(existingContact);
                        transaction.Commit();

                        return Json(new { success = true, message = "Contact edited successfully" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(500, "Error editing contact: " + ex.Message);
                    }
                }
            }
        }


        [HttpPost]
        public ActionResult UpdateContactStatus(Guid contactId, bool isActive)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var contact = session.Get<Contact>(contactId);
                    if (contact != null)
                    {
                        contact.IsActive = isActive;
                        session.Update(contact);
                        transaction.Commit();
                    }
                }
            }

            return Json(new { success = true });
        }

    }
}
