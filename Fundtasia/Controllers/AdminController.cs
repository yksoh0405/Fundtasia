using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Fundtasia.Controllers
{
    public class AdminController : Controller
    {
        //This controller is to handle the request from the webpage that need certain information
        DBEntities1 db = new DBEntities1();

        // GET: Admin
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Dashboard()
        {
            ViewBag.TotalEventCreated = db.Events.Count();
            ViewBag.TotalEventViews = db.Events.Sum(s => s.View);
            ViewBag.TotalDonationEarned = db.Donations.Sum(s => s.Amount);
            return View();
        }

        public ActionResult DashboardDonation()
        {
            var dt = db.Donations.GroupBy(s => s.EventId).ToList().Select(g => new object[] { g.Key, g.Count() });

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UsersData()
        {
            using (DBEntities1 da = new DBEntities1())
            {
                var udt = da.Users.GroupBy(s => s.Role).ToList().Select(g => new object[] { g.Key, g.Count() });

                return Json(udt, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MerchandiseData()
        {
            using (DBEntities1 da = new DBEntities1())
            {
                var dt = da.Merchandises.GroupBy(s => s.Status).ToList().Select(g => new object[] { g.Key, g.Count() });

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EventData()
        {
            var dt = db.Events.ToList().OrderBy(g => g.View).Select(g => new object[] { g.View, g.Id }).Take(3);

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult ViewProfile()
        {
            User userSession = (User)Session["UserSession"];
            User loginUser = db.Users.Find(userSession.Id);

            var model = new StaffProfileVM
            {
                Id = loginUser.Id,
                Email = loginUser.Email,
                Role = loginUser.Role,
                FirstName = loginUser.FirstName,
                LastName = loginUser.LastName,
                Status = loginUser.Status,
                LastLoginTime = loginUser.LastLoginTime,
                LastLoginIP = loginUser.LastLoginIP
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult ViewProfile(StaffProfileVM model)
        {
            using (DBEntities1 da = new DBEntities1())
            {
                var s = db.Users.Find(model.Id);

                if (s == null)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                if (ModelState.IsValid)
                {
                    //If Email string is different with database
                    if (!String.Equals(s.Email, model.Email))
                    {
                        //Check email exist in another record
                        if (IsEmailExist(model.Email))
                        {
                            ModelState.AddModelError("EmailExist", "Email Already Exist");
                            return View(model);
                        }
                        else
                        {
                            s.Email = model.Email;
                        }
                    }
                    s.FirstName = model.FirstName;
                    s.LastName = model.LastName;
                    if (model.PasswordHash != null)
                    {
                        s.PasswordHash = Crypto.Hash(model.PasswordHash);
                    }
                    da.SaveChanges();

                    Session["UserSession"] = new User(s.Id, s.Email, s.Role, s.FirstName, s.LastName, s.Status, (DateTime)s.LastLoginTime, s.LastLoginIP);
                    TempData["Info"] = $"{s.FirstName} data saved.";
                    return RedirectToAction("Dashboard", "Admin");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult ViewMerchandiseSale(Guid id)
        {
            var model = db.UserMerchandises.Find(id);
            Dictionary<string, string> state = State();
            model.State = state[model.State];
            return View(model);
        }

        [NonAction]
        public bool IsEmailExist(string email)
        {
            //Bring DB to find the existing email
            using (db)
            {
                //FirstOrDefault() only return one record
                var v = db.Users.Where(s => s.Email == email).FirstOrDefault();
                return v != null;
            }
        }

        public Dictionary<string, string> State()
        {
            Dictionary<string, string> state = new Dictionary<string, string>(){
                {"JH", "Johor"},
                {"KD", "Kedah"},
                {"KT", "Kelantan"},
                {"MK", "Malacca"},
                {"NS", "Negeri Sembilan"},
                {"PH", "Pahang"},
                {"PN", "Penang"},
                {"PR", "Perak"},
                {"PL", "Perlis"},
                {"SB", "Sabah"},
                {"SW", "Sarawak"},
                {"SG", "Selangor"},
                {"TR", "Terengganu"}
            };
            return state;
        }
    }
}