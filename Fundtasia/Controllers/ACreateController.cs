using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundtasia.Controllers
{
    public class ACreateController : Controller
    {
        DBEntities1 db = new DBEntities1();

        //This controller is used to handle the Create request from View to Model
        public ActionResult CreateStaff()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateStaff(User model)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(model);
                db.SaveChanges();

                return RedirectToAction("Staff", "AList");
            }

            return View(model);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        //Create Event Action
        [HttpGet]
        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(Event model)
        {
            return View(model);
        }

        public ActionResult CreateMerchandise()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMerchandise(Merchandise model)
        {
            if(ModelState.IsValid)
            {
                db.Merchandises.Add(model);
                db.SaveChanges();
                TempData["Info"] = "Merchandise added.";

                return RedirectToAction("Merchandise", "AList");
            }
            
            return View(model);
        }

        public ActionResult CreateReport()
        {
            return View();
        }
    }
}