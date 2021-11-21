using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Security.Cryptography;

namespace Fundtasia.Controllers
{
    public class HomeController : Controller
    {
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

        public ActionResult SignUp(User user)
        {


            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}