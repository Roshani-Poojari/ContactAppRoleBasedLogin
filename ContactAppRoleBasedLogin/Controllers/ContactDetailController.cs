using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppRoleBasedLogin.Data;
using ContactAppRoleBasedLogin.Models;

namespace ContactAppRoleBasedLogin.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ContactDetailController : Controller
    {
        // GET: ContactDetail
        public ActionResult Index(Guid contactId)
        {
            TempData["ContactId"] = contactId;
            return View();
        }
        public ActionResult GetContactDetails(int page, int rows, string sidx, string sord, bool _search, string searchField,string searchString, string searchOper)
        {
            Guid contactId = (Guid)TempData.Peek("ContactId");
            using (var session = NHibernateHelper.CreateSession())
            {
                var contactDetails = session.Query<ContactDetail>().Where(cd => cd.Contact.Id == contactId).ToList();
                var contactDetailList = contactDetails;

                //check if search operation was requested
                if(_search && searchField == "Email" && searchOper == "eq")
                {
                    contactDetailList = contactDetails.Where(cd=>cd.Email == searchString).ToList();
                }

                //Get total count of records(for pagination)
                int totalCount = contactDetails.Count();
                //Calculate total pages
                int totalPages = (int)Math.Ceiling((double)totalCount / rows);

                //for sorting
                switch (sidx)
                {
                    case "Number":
                        contactDetailList = sord == "asc" ? contactDetailList.OrderBy(cd=>cd.Number).ToList()
                            : contactDetailList.OrderByDescending(cd=>cd.Number).ToList();
                        break;
                    case "Email":
                        contactDetailList = sord == "asc" ? contactDetailList.OrderBy(cd => cd.Email).ToList()
                            : contactDetailList.OrderByDescending(cd => cd.Email).ToList();
                        break;
                }


                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalCount,
                    rows = contactDetailList.Select(cd=> new
                    {
                        cell = new string[]
                        {
                            cd.Id.ToString(),
                            cd.Number.ToString(),
                            cd.Email
                        }
                    }).Skip(page - 1 * rows).Take(rows).ToArray()
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult AddContactDetail(ContactDetail contactDetail)
        {
            Guid contactId = (Guid)TempData["ContactId"];
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var contact = session.Query<Contact>().SingleOrDefault(c => c.Id == contactId);
                    contactDetail.Contact = contact;
                    session.Save(contactDetail);
                    transaction.Commit();
                    return Json(new { success = true, message = "Contact Detail added successfully" });
                }
            }
        }
        public ActionResult DeleteContactDetail(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var contactDetail = session.Query<ContactDetail>().FirstOrDefault(cd => cd.Id == id);
                    session.Delete(contactDetail);
                    transaction.Commit();
                    return Json(new { success = true, message = "Contact Detail deleted successfully" });
                }
            }
        }
        public ActionResult EditContactDetail(ContactDetail contactDetail)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var existingContactDetail = session.Query<ContactDetail>().FirstOrDefault(cd => cd.Id == contactDetail.Id);
                    if (existingContactDetail != null)
                    {
                        existingContactDetail.Number = contactDetail.Number;
                        existingContactDetail.Email = contactDetail.Email;
                    }
                    session.Update(existingContactDetail);
                    transaction.Commit();
                    return Json(new { success = true, message = "Contact Detail edited successfully." });

                }
            }
        }
    }
}