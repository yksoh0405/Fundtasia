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
            CheckAuth();
            return View();
        }

        public ActionResult ReportDetails()
        {
            return View();
        }

        public ActionResult ViewProfile()
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

        //This code is to check login session to avoid the logined user go to other page
        [NonAction]
        public void CheckAuth()
        {
            if (!Request.IsAuthenticated)
            {
                User loginUser = (User)Session["UserSession"];
                if(loginUser.Role == "User")
                {
                    RedirectToAction("Index", "Home");
                }
                RedirectToAction("Index", "Home");
            }
        }
    }
}