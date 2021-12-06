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
            ViewBag.EventList = new SelectList(db.Events, "Id", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult DonationPayment(Donation model)
        {
            if (ModelState.IsValid)
            {
                model.TimeDonated = DateTime.Now;
                db.Donations.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventList = new SelectList(db.Donations, "Id", "Title");
            return View(model);
        }

        public ActionResult DonationReceipt()
        {
            //pass img, price and name from donation payment
            return View();
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

        public ActionResult Receipt()
        {
            //pass img, price and name from merch payment
            return View();
        }

        public ActionResult Event(string sort, string sortdir, int page = 1)
        {
            Func<Event, object> fn = s => s.Id;

            switch (sort)
            {
                case "CreatedTime": fn = s => s.CreatedDate; break;
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