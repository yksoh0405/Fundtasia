using Fundtasia.Models;
using System;
using System.Collections.Generic;
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

        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult CreateEvent()
        {
            return View();
        }

        public ActionResult CreateMerchandise()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMerchandise(Merchandise model)
        {
            return View(model);
        }

        public ActionResult CreateReport()
        {
            return View();
        }
    }
}