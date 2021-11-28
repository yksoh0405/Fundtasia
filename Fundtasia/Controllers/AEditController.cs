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

        // GET: AEdit
        public ActionResult EditMerchandise(string Id)
        {
            var model = db.Merchandises.Find(Id);
            if(model == null)
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

            if(m == null)
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