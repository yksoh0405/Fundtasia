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
        public ActionResult Dashboard()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["UserSession"] != null)
            {
                User loginUser = (User)Session["UserSession"];
                if (String.Equals(loginUser.Role, "User"))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult ReportDetails(string id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["UserSession"] != null)
            {
                User loginUser = (User)Session["UserSession"];
                if (String.Equals(loginUser.Role, "User"))
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var model = db.Reports.Find(id);

            return View(model);
        }

        public ActionResult ViewProfile()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            User loginUser = null;

            if (Session["UserSession"] != null)
            {
                loginUser = (User)Session["UserSession"];
                if (String.Equals(loginUser.Role, "User"))
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            if (loginUser == null)
            {
                return RedirectToAction("LogIn", "UserAuth");

            }

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
        public ActionResult ViewProfile(StaffProfileVM model)
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
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Admin");
            }

            return View(model);
        }

        public ActionResult ViewMerchandiseSale(Guid id)
        {
            var model = db.UserMerchandises.Find(id);
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
    }
}