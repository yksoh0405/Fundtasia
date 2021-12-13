﻿using System;
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
using System.Data.Entity.Validation;

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
            var model = db.Merchandises;

            return View(model);
        }

        [Authorize]
        public ActionResult DonationPayment()
        {
            //Retrieve the event list order by to the least amount donation of the event
            //SELECT e.Id, e.Title, SUM(d.Amount)
            //FROM Event e JOIN Donation d ON e.Id = d.EventId
            //GROUP BY e.Id
            //ORDER BY SUM(d.Amount)
            ViewBag.EventList = new SelectList(db.Events.OrderBy(e => e.Donations.Count()), "Id", "Title");
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult DonationPayment(Donation model)
        {
            if (ModelState.IsValid)
            {
                User loginUser = (User)Session["UserSession"];

                var d = new Donation
                {
                    Id = Guid.NewGuid(),
                    UserId = loginUser.Id,
                    TimeDonated = DateTime.Now,
                    Amount = model.Amount,
                    EventId = model.EventId
                };

                db.Donations.Add(d);
                db.SaveChanges();
                return RedirectToAction("DonationReceipt", "Home", new { Id = d.Id });
            }

            ViewBag.EventList = new SelectList(db.Donations, "Id", "Title");
            return View(model);
        }

        [Authorize]
        public ActionResult DonationReceipt(Guid Id)
        {
            var model = db.Donations.Find(Id);
            return View(model);
        }

        [Authorize]
        public ActionResult MerchandisePayment(string Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Donation", "Home");
            }

            ViewBag.EventList = new SelectList(db.Events, "Id", "Title");
            ViewBag.merchandise = db.Merchandises.Find(Id);
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult MerchandisePayment(MerchPaymentVM model)
        {
            if (ModelState.IsValid)
            {
                var merchandise = db.Merchandises.Find(model.MerchandiseId);
                User loginUser = (User)Session["UserSession"];
                var um = new UserMerchandise
                {
                    Id = Guid.NewGuid(),
                    UserId = loginUser.Id,
                    MerchandiseId = merchandise.Id,
                    Price = merchandise.Price,
                    PurchaseTime = DateTime.Now,
                    Size = model.Size,
                    FullName = model.FullName,
                    StreetAddress = model.StreetAddress,
                    State = model.State,
                    City = model.City,
                    PostalCode = model.PostalCode
                };

                db.UserMerchandises.Add(um);
                db.SaveChanges();

                return RedirectToAction("MerchandiseReceipt", "Home", new { Id = um.Id });
            }

            return View(model);
        }

        [Authorize]
        public ActionResult MerchandiseReceipt(Guid Id)
        {
            var model = db.UserMerchandises.Find(Id);
            Dictionary<string, string> state = State();
            model.State = state[model.State];
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
            if (id == null)
            {
                return RedirectToAction("Event", "Home");
            }
            var model = db.Events.Find(id);
            model.View = model.View + 1;
            db.SaveChanges();

            var donation = db.Donations.Where(s => s.EventId.Contains(id));

            if (db.Donations.FirstOrDefault(s => s.EventId.Contains(id)) != null)
            {
                ViewBag.TotalDonation = donation.Select(s => s.Amount).Sum();
                ViewBag.TotalDonor = donation.Select(s => s.UserId).Distinct().Count();
            }

            return View(model);
        }

        public Dictionary<string, string> State()
        {
            Dictionary<string, string> state = new Dictionary<string, string>(){
                {"JH", "Johor"},
                {"KD", "Kedah"},
                {"KT", "Kelantan"},
                {"MK", "Malacca"},
                {"NS", "Negeri Sembilan"},
                {"PH", "Pahang"},
                {"PN", "Penang"},
                {"PR", "Perak"},
                {"PL", "Perlis"},
                {"SB", "Sabah"},
                {"SW", "Sarawak"},
                {"SG", "Selangor"},
                {"TR", "Terengganu"}
            };
            return state;
        }
    }
}