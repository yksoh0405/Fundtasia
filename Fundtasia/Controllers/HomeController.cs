using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using Fundtasia.Models;
using System.Net.Sockets;
using System.Dynamic;

namespace Fundtasia.Controllers
{
    public class HomeController : Controller
    {
        //This controller is to handle the request from the webpage that need certain information
        DBEntities1 db = new DBEntities1();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Donation()
        {
            if (!Request.IsAuthenticated)
            {
                RedirectToAction("LogIn", "UserAuth");
            }

            var model = db.Merchandises;
            return View(model);
        }

        public ActionResult DonationPayment()
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

            ViewBag.EventList = new SelectList(db.Events, "Id", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult DonationPayment(Donation model)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["UserSession"] != null)
            {
                User loginUser = (User)Session["UserSession"];
                model.UserId = loginUser.Id;
                if (String.Equals(loginUser.Role, "User"))
                {
                    return RedirectToAction("Index", "Home");
                } 
            }

            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid();

                var d = new Donation
                {
                    Id = model.Id,
                    UserId = model.UserId,
                    TimeDonated = DateTime.Now,
                    Amount = model.Amount,
                    EventId = model.EventId
                };

                db.Donations.Add(d);
                db.SaveChanges();
                return RedirectToAction("DonationReceipt", "Home");
            }

            ViewBag.EventList = new SelectList(db.Donations, "Id", "Title");
            return View(model);
        }

        public ActionResult DonationReceipt(string sort = "Time Donated", string sortdir = "DESC", int page = 1, string keyword = "")
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
                case "Donor": fn = d => d.User.FirstName; break;
                case "Time Donated": fn = d => d.TimeDonated; break;
                case "Amount": fn = d => d.Amount; break;
                case "Event": fn = d => d.EventId; break;
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
            if (Request.IsAjaxRequest()) return PartialView("_DonationReceipt", model);

            return View(model);
        }

        public ActionResult MerchandisePayment(string id)
        {
            var model = db.Merchandises.Find(id);

            if (model == null)
            {
                return RedirectToAction("Receipt");
            }

            return View(model);
        }

        public ActionResult MerchandiseReceipt(string sort = "Time Donated", string sortdir = "DESC", int page = 1, string keyword = "")
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

            Func<UserMerchandise, object> fn = d => d.Id;
            switch (sort)
            {
                case "Id": fn = d => d.Id; break;
            }

            var sorted = sortdir == "DESC" ? db.UserMerchandises.Where(s => s.User.FirstName.Contains(keyword)).OrderByDescending(fn) : db.UserMerchandises.Where(s => s.User.FirstName.Contains(keyword)).OrderBy(fn);

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
            if (Request.IsAjaxRequest()) return PartialView("_MerchandiseReceipt", model);

            return View(model);
        }

        public ActionResult Event(string sort = "CreatedDate", string sortdir = "DESC", int page = 1)
        {
            Func<Event, object> fn = s => s.Id;

            switch (sort)
            {
                case "CreatedDate": fn = s => s.CreatedDate; break;
                case "Title": fn = s => s.Title; break;
                case "View": fn = s => s.View; break;
            }

            var sorted = sortdir == "DESC" ? db.Events.OrderByDescending(fn) : db.Events.OrderBy(fn);

            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            return View(model);
        }

        public ActionResult EventDetail(string id)
        {
            if(id == null)
            {
                return RedirectToAction("Event", "Home");
            }
            var model = db.Events.Find(id);
            model.View = model.View + 1;
            db.SaveChanges();
            return View(model);
        }
    }
}