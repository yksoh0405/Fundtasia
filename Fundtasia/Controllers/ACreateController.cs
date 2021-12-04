using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Fundtasia.Controllers
{
    public class ACreateController : Controller
    {
        DBEntities1 db = new DBEntities1();

        //This controller is used to handle the Create request from View to Model
        public ActionResult CreateStaff()
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
            return View();
        }

        [HttpPost]
        public ActionResult CreateStaff(User model)
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
            if (ModelState.IsValid)
            {
                db.Users.Add(model);
                db.SaveChanges();

                return RedirectToAction("Staff", "AList");
            }

            return View(model);
        }

        public ActionResult CreateUser()
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
            return View();
        }

        //Create Event Action
        [HttpGet]
        public ActionResult CreateEvent()
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
            return View();
        }

        [HttpPost]
        public ActionResult CreateEvent(EventInsertVM model)
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

            string err = ValidatePhoto(model.CoverImage);

            if (ModelState.IsValid)
            {
                //Generate Id
                string max = db.Events.Max(ev => ev.Id) ?? "E000";
                int n = int.Parse(max.Substring(1));
                string id = (n + 1).ToString("'E'000");
                int endIndex = model.YouTubeLink.Length;
                int startIndex = endIndex - 11;
                model.YouTubeLink = model.YouTubeLink.Substring(startIndex);

                var e = new Event
                {
                    Id = id,
                    UserId = model.UserId,
                    Title = model.Title,
                    View = 0,
                    Likes = 0,
                    CoverImage = SaveEventPhoto(model.CoverImage),
                    Target = model.Target,
                    YouTubeLink = model.YouTubeLink,
                    CreatedDate = DateTime.Now,
                    Article = model.Article
                };

                db.Events.Add(e);
                db.SaveChanges();
                TempData["Message"] = "Event created";
                return RedirectToAction("Event", "AList");
            }
            return View(model);
        }

        public ActionResult CreateMerchandise()
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
            return View();
        }

        [HttpPost]
        public ActionResult CreateMerchandise(Merchandise model)
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
            if (ModelState.IsValid)
            {
                db.Merchandises.Add(model);
                db.SaveChanges();
                TempData["Info"] = "Merchandise added.";

                return RedirectToAction("Merchandise", "AList");
            }
            
            return View(model);
        }

        public ActionResult CreateReport()
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
            return View();
        }

        [HttpPost]
        public ActionResult CreateReport(Report model)
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
            return View();
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
            string path = Server.MapPath($"~/Uploads/Event/{name}");

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
            string path = Server.MapPath($"~/Uploads/Merchandise/{name}");

            //Image resizing
            var img = new WebImage(f.InputStream);

            if (img.Width > img.Height)
            {
                //Crop the image width when image width > image height
                int px = (img.Width - img.Height) / 2;
                img.Crop(0, px, 0, px);
                //Crop(Top, Left, Bottom, Right)
            }

            if (img.Width < img.Height)
            {
                //Crop the image width when image width < image height
                int px = (img.Height - img.Width) / 2;
                img.Crop(px, 0, px, 0);
            }

            img.Resize(296, 295).Crop(1, 1).Save(path, "jpeg");

            return name;
        }

    }
}