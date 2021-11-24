using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Fundtasia.Controllers
{
    public class AListController : Controller
    {
        //This controller is used to handle a list of record between Model and View
        //Apply AJAX search, sorting, paging skill from P5 Demo 5 + 2(For highlighting purpose)
        //Need to use the skill from P4 Demo 7 WebGrid
        DBEntities1 db = new DBEntities1();

        public ActionResult Staff()
        {
            var model = db.Users.ToList();
            return View(model);
        }

        public ActionResult ClientUser()
        {
            var model = db.Users.ToList();
            return View(model);
        }

        public ActionResult Event()
        {
            var model = db.Events;
            return View(model);
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
    }
}