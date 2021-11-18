using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundtasia.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Staff()
        {
            return View();
        }

        //Actually this is the user page, but because of we use the "User" word to become the method it will have the conflict with the Controller.User
        public ActionResult ClientUser()
        {
            return View();
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

    }
}