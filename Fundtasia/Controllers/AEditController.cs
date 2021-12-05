using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
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
        public ActionResult EditEvent(EventEditVM model)
        {
            var m = db.Events.Find(model.Id);
            if (m == null)
            {
                return RedirectToAction("Event", "AList");  //No record found
            }

            if (model.CoverImage != null)
            {
                string err = ValidatePhoto(model.CoverImage);
                if (err != null)
                {
                    ModelState.AddModelError("CoverImage", err);
                }
            }

            if (ModelState.IsValid)
            {
                int endIndex = model.YouTubeLink.Length;
                int startIndex = endIndex - 11;
                model.YouTubeLink = model.YouTubeLink.Substring(startIndex);

                m.Title = model.Title;
                if (model.CoverImage != null)
                {
                    DeleteEventPhoto(m.CoverImage);
                    m.CoverImage = SaveEventPhoto(model.CoverImage);
                }
                m.YouTubeLink = model.YouTubeLink;
                m.Article = model.Article;
                db.SaveChanges();
                return RedirectToAction("Event", "AList");
            }
            model.ImageURL = m.CoverImage;
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
                    ModelState.AddModelError("ImageURL", err);
                }
            }

            if (ModelState.IsValid)
            {
                m.Name = model.Name;

                if (model.ImageURL != null)
                {
                    DeleteMerchandisePhoto(m.Image);
                    m.Image = SaveMerchandisePhoto(model.ImageURL);
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

        private string SaveEventPhoto(HttpPostedFileBase f)
        {
            //Generate Unique Id
            //Save the image in the format of .jpg
            string name = Guid.NewGuid().ToString("n") + ".jpg";
            string path = Server.MapPath($"~/Images/Uploads/Event/{name}");

            //Image resizing
            var img = new WebImage(f.InputStream);

            img.Resize(513, 316).Crop(1, 1).Save(path, "jpeg");

            return name;
        }

        private string SaveMerchandisePhoto(HttpPostedFileBase f)
        {
            //Generate Unique Id
            //Save the image in the format of .jpg
            string name = Guid.NewGuid().ToString("n") + ".jpg";
            string path = Server.MapPath($"~/Images/Uploads/Merchandise/{name}");

            //Image resizing
            var img = new WebImage(f.InputStream);

            img.Resize(296, 295).Crop(1, 1).Save(path, "jpeg");

            return name;
        }

        private void DeleteEventPhoto(string name)
        {
            name = System.IO.Path.GetFileName(name);
            string path = Server.MapPath($"~/Images/Uploads/Event/{name}");
            System.IO.File.Delete(path);
        }

        private void DeleteMerchandisePhoto(string name)
        {
            name = System.IO.Path.GetFileName(name);
            string path = Server.MapPath($"~/Images/Uploads/Merchandise/{name}");
            System.IO.File.Delete(path);
        }
    }
}