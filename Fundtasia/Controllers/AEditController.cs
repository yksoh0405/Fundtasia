using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundtasia.Controllers
{
    public class AEditController : Controller
    {
        DBEntities1 db = new DBEntities1();

        // Edit Staff
        public ActionResult EditStaff(string email)
        {
            var model = db.Users.Find(email);
            if (model == null)
            {
                return RedirectToAction("Staff", "AList");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditStaff(User model)
        {
            var s = db.Users.Find(model.Email);

            if (s == null)
            {
                return RedirectToAction("Staff", "AList");
            }

            if (ModelState.IsValid)
            {
                s.LastName = model.LastName;
                s.FirstName = model.FirstName;
                s.Role = model.Role;
                s.Status = model.Status;
                db.SaveChanges();

                return RedirectToAction("Staff", "AList");
            }

            return View(model);
        }

        // Edit Client
        public ActionResult EditClientUser(string email)
        {
            var model = db.Users.Find(email);
            if (model == null)
            {
                return RedirectToAction("ClientUser", "AList");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditClientUser(User model)
        {
            var cu = db.Users.Find(model.Email);

            if (cu == null)
            {
                return RedirectToAction("ClientUser", "AList");
            }

            if (ModelState.IsValid)
            {
                cu.LastName = model.LastName;
                cu.FirstName = model.FirstName;
                cu.Status = model.Status;
                db.SaveChanges();

                return RedirectToAction("ClientUser", "AList");
            }

            return View(model);
        }

        // Edit Merchandise
        // GET: AEdit
        public ActionResult EditMerchandise(string Id)
        {
            var model = db.Merchandises.Find(Id);
            if (model == null)
            {
                return RedirectToAction("Merchandise", "AList");   //no record found
            }

            return View(model);
        }

        // POST: AEdit
        [HttpPost]
        public ActionResult EditMerchandise(Merchandise model)
        {
            var m = db.Merchandises.Find(model.Id);  //find record with id

            if (m == null)
            {
                return RedirectToAction("Merchandise", "AList");  //no record found
            }

            if (ModelState.IsValid)
            {
                m.Name = model.Name;
                m.Price = model.Price;
                m.Status = model.Status;
                db.SaveChanges();

                TempData["Info"] = "Merchandise edited.";
                return RedirectToAction("Merchandise", "AList");
            }

            return View(model);
        }
    }
}