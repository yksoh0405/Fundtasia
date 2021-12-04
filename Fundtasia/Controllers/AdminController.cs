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