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

        public ActionResult Staff(string sort = "LastLoginTime", string sortdir = "DESC", int page = 1, string keyword = "")
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

            keyword = keyword.Trim();

            //Sorting
            Func<User, object> fn = s => s.Id;

            switch (sort)
            {
                case "FirstName": fn = s => s.FirstName; break;
                case "LastName": fn = s => s.LastName; break;
                case "Verified": fn = s => s.IsEmailVerified; break;
                case "Status": fn = s => s.Status; break;
                case "LastLoginTime": fn = s => s.LastLoginTime; break;
                case "Role": fn = s => s.Role; break;
            }

            var sorted = sortdir == "DESC" ? db.Users.Where(s => s.FirstName.Contains(keyword)).OrderByDescending(fn) : db.Users.Where(s => s.FirstName.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_Staff", model);

            return View(model);
        }

        public ActionResult ClientUser(string sort = "LastLoginTime", string sortdir = "DESC", int page = 1, string keyword = "")
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

            keyword = keyword.Trim();

            //Sorting
            Func<User, object> fn = s => s.Id;

            switch (sort)
            {
                case "FirstName": fn = s => s.FirstName; break;
                case "LastName": fn = s => s.LastName; break;
                case "Verified": fn = s => s.IsEmailVerified; break;
                case "Status": fn = s => s.Status; break;
                case "LastLoginTime": fn = s => s.LastLoginTime; break;
                case "LastIP": fn = s => s.LastLoginIP; break;
            }

            var sorted = sortdir == "DESC" ? db.Users.Where(s => s.FirstName.Contains(keyword)).OrderByDescending(fn) : db.Users.Where(s => s.FirstName.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_ClientUser", model);

            return View(model);
        }

        public ActionResult Event(string sort = "CreatedDate", string sortdir = "DESC", int page = 1, string keyword = "")
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

            keyword = keyword.Trim();

            //Sorting
            Func<Event, object> fn = s => s.Id;

            switch (sort)
            {
                case "Id": fn = s => s.Id; break;
                case "CreatedDate": fn = s => s.CreatedDate; break;
                case "Title": fn = s => s.Title; break;
                case "View": fn = s => s.View; break;
            }

            var sorted = sortdir == "DESC" ? db.Events.Where(s => s.Title.Contains(keyword)).OrderByDescending(fn) : db.Events.Where(s => s.Title.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_Event", model);
            
            return View(model);
        }

        public ActionResult Merchandise(string sort = "Name", string sortdir = "ASC", int page = 1, string keyword = "")
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

            keyword = keyword.Trim();

            Func<Merchandise, object> fn = m => m.Id;
            switch (sort)
            {
                case "Id": fn = m => m.Id; break;
                case "Name": fn = m => m.Name; break;
                case "Price": fn = m => m.Price; break;
            }

            var sorted = sortdir == "DESC" ? db.Merchandises.Where(s => s.Name.Contains(keyword)).OrderByDescending(fn) : db.Merchandises.Where(s => s.Name.Contains(keyword)).OrderBy(fn);

            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            if (Request.IsAjaxRequest()) return PartialView("_Merchandise", model);

            return View(model);
        }

        public ActionResult Donation(string sort = "TimeDonated", string sortdir = "DESC", int page = 1, string keyword = "")
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
            
            Func<Donation, object> fn = d => d.Id;
            switch (sort)
            {
                case "Id": fn = d => d.Id; break;
                case "UserId": fn = d => d.User.LastName; break;
                case "TimeDonated": fn = d => d.TimeDonated; break;
                case "Amount": fn = d => d.Amount; break;
                case "EventId": fn = d => d.EventId; break;
            }

            var sorted = sortdir == "DESC" ? db.Donations.Where(s => s.User.FirstName.Contains(keyword)).OrderByDescending(fn) : db.Donations.Where(s => s.User.FirstName.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_Donation", model);

            return View(model);
        }

        public ActionResult Report(string sort = "CreatedDate", string sortdir = "ASC", int page = 1, string keyword = "")
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
            
            keyword = keyword.Trim();

            //Sorting
            Func<Report, object> fn = s => s.Id;

            switch (sort)
            {
                case "Id": fn = s => s.Id; break;
                case "Title": fn = s => s.Title; break;
                case "CreatedDate": fn = s => s.CreatedDate; break;
                case "CreatedBy": fn = s => s.User.LastName; break;
            }

            var sorted = sortdir == "DESC" ? db.Reports.Where(s => s.Title.Contains(keyword)).OrderByDescending(fn) : db.Reports.Where(s => s.Title.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_Report", model);

            return View(model);
        }
    }
}