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
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult Receipt()
        {
            return View();
        }

        public ActionResult Event()
        {
            return View();
        }

        public ActionResult EventDetail()
        {
            return View();
        }
    }
}