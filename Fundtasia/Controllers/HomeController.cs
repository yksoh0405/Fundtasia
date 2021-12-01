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

        public ActionResult MerchandisePayment(string id)
        {
            var model = db.Merchandises.Find(id);

            if (model == null)
            {
                return RedirectToAction("Receipt");
            }

            return View(model);
        }

        public ActionResult DonationPayment()
        {
            return View();
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

            var sorted = sortdir == "ASC" ? db.Events.OrderBy(fn) : db.Events.OrderByDescending(fn);

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

            return View(model);
        }
    }
}