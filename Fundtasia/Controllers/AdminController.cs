using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fundtasia.Models;

namespace Fundtasia.Controllers
{
    public class AdminController : Controller
    {
        DBEntities1 db = new DBEntities1();

        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Staff()
        {
            var model = db.Users;
            return View(model);
        }

        //Actually this is the user page, but because of we use the "User" word to become the method it will have the conflict with the Controller.User
        public ActionResult ClientUser()
        {
            var model = db.Users;
            return View(model);
        }

        public ActionResult Event()
        {
            return View();
        }

        public ActionResult Merchandise()
        {
            return View();
        }

        public ActionResult Donation()
        {
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }

        public ActionResult ReportDetails()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            using (DBEntities1 dc = new DBEntities1())
            {
                var events = dc.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}