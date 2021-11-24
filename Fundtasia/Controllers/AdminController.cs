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
            return View();
        }

        

        public ActionResult ReportDetails()
        {
            return View();
        }

    }
}