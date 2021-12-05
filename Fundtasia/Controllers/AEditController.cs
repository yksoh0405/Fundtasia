using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Fundtasia.Controllers
{
    public class AEditController : Controller
    {
        DBEntities1 db = new DBEntities1();

        // Edit Staff
        public ActionResult EditStaff(Guid Id)
        {
            var model = db.Users.Find(Id);
            if (model == null)
            {
                return RedirectToAction("Staff", "AList");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditStaff(User model)
        {
            var s = db.Users.Find(model.Id);

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
        public ActionResult EditClientUser(Guid Id)
        {
            var model = db.Users.Find(Id);
            if (model == null)
            {
                return RedirectToAction("ClientUser", "AList");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditClientUser(User model)
        {
            var cu = db.Users.Find(model.Id);

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

        [HttpGet]
        public ActionResult EditEvent(string id)
        {
            var m = db.Events.Find(id);
            if(m == null)
            {
                return RedirectToAction("Event", "AList");
            }

            var model = new EventEditVM
            {
                Title = m.Title,
                ImageURL = m.CoverImage,
                Target = m.Target,
                YouTubeLink = "https://youtu.be/" + m.YouTubeLink,
                Article = m.Article
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditEvent(Event model)
        {
            var m = db.Events.Find(model.Id);
            if (m == null)
            {
                return RedirectToAction("Event", "AList");  //No record found
            }

            if (ModelState.IsValid)
            {
                int endIndex = model.YouTubeLink.Length;
                int startIndex = endIndex - 11;
                model.YouTubeLink = model.YouTubeLink.Substring(startIndex);

                m.Title = model.Title;
                m.CoverImage = model.CoverImage;
                m.YouTubeLink = model.YouTubeLink;
                m.Article = model.Article;
                db.SaveChanges();
            }

            return View(model);
        }

        // Edit Merchandise
        // GET: AEdit
        public ActionResult EditMerchandise(string Id)
        {
            var m = db.Merchandises.Find(Id);
            if (m == null)
            {
                return RedirectToAction("Merchandise", "AList");
            }

            var model = new MerchandiseEditVM
            {
                Id = m.Id,
                Name = m.Name,
                Image = m.Image,
                Price = m.Price,
                Status = m.Status
            };

            return View(model);
        }

        // POST: AEdit
        [HttpPost]
        public ActionResult EditMerchandise(MerchandiseEditVM model)
        {

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

            // TODO
            var m = db.Merchandises.Find(model.Id);
            if (m == null)
            {
                return RedirectToAction("Merchandise", "AList");
            }

            if (model.ImageURL != null)
            {
                string err = ValidatePhoto(model.ImageURL);
                if (err != null)
                {
                    ModelState.AddModelError("Photo", err);
                }
            }

            if (ModelState.IsValid)
            {
                m.Name = model.Name;
                m.Email = model.Email;

                if (model.Photo != null)
                {
                    DeletePhoto(m.PhotoURL);
                    m.PhotoURL = SavePhoto(model.Photo);
                }

                db.SaveChanges();
                TempData["Info"] = "Merchandise edited";
                return RedirectToAction("Merchandise", "AList");
            }
            model.Image = m.Image;
            return View(model);
        }

        private string ValidatePhoto(HttpPostedFileBase f)
        {
            var reType = new Regex(@"^image\/(jpeg|png)$", RegexOptions.IgnoreCase);
            var reName = new Regex(@"^.+\.(jpg|jpeg|png)$", RegexOptions.IgnoreCase);

            if (f == null)
            {
                return "No image inserted";
            }
            else if (!reType.IsMatch(f.ContentType) || !reName.IsMatch(f.FileName))
            {
                return "Only JPG or PNG image is allowed";
            }
            else if (f.ContentLength > 1 * 1024 * 1024)
            {
                // nmb = n * 1024 * 1024
                return "Image size exceeded 1Mb";
            }
            return null;
        }
    }
}