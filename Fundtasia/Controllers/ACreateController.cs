using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundtasia.Controllers
{
    public class ACreateController : Controller
    {
        DBEntities1 db = new DBEntities1();

        //This controller is used to handle the Create request from View to Model
        public ActionResult CreateStaff()
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        //Create Event Action
        [HttpGet]
        public ActionResult CreateEvent(Event model)
        {
            if (Request.IsAjaxRequest())
            {
                int position = model.YouTubeLink.IndexOf("https://youtu.be/");
                model.YouTubeLink = model.YouTubeLink.Substring(position);
                return PartialView("_EventPartial", model);
            }
            return View();
        }

        //Create Event Post Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {

            foreach (var file in files)
            {
                string filePath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath("~/Images/Uploads/Event"), filePath));
                //Here you can write code for save this information in your database if you want
            }
            return Json("File uploaded successfully");
        }

        public ActionResult CreateMerchandise()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMerchandise(Merchandise model)
        {
            return View(model);
        }

        public ActionResult CreateReport()
        {
            return View();
        }
    }
}