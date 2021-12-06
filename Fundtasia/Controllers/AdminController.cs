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

        public ActionResult ReportDetails()
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

            var model = db.Users.Find(loginUser.Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult ViewProfile(User model)
        {
            var s = db.Users.Find(model.Id);

            if (s == null)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            if (ModelState.IsValid)
            {
                if (s.Email != model.Email)
                {
                    if (IsEmailExist(model.Email))
                    {
                        ModelState.AddModelError("EmailExist", "Email Already Exist");
                        return View();
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