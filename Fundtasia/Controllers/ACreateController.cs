using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult CreateStaff()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        public ActionResult CreateStaff(CreateUserVM model)
        {
            if (ModelState.IsValid)
            {
                // Check email exist
                if (IsEmailExist(model.Email))
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exist");
                    return View(model);
                }

                var newStaff = new User
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    Role = model.Role,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PasswordHash = Crypto.Hash(model.PasswordHash),
                    IsEmailVerified = model.IsEmailVerified,
                    ActivationCode = Guid.NewGuid(),
                    Status = "Active"
                };

                using (DBEntities1 da = new DBEntities1())
                {
                    da.Users.Add(newStaff);
                    da.SaveChanges();

                    if (!newStaff.IsEmailVerified)
                    {
                        SendVerificaionLinkEmail(newStaff.Email, newStaff.ActivationCode.ToString());
                    }
                }

                return RedirectToAction("Staff", "AList");
            }

            return View(model);
        }

        public ActionResult CreateClientUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateClientUser(CreateUserVM model)
        {
            if (ModelState.IsValid)
            {
                // Check email exist
                var isExist = IsEmailExist(model.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exist");
                    return View(model);
                }

                var newClientUser = new User
                {
                    // Password Hash
                    PasswordHash = Crypto.Hash(model.PasswordHash),

                    // Activation Code
                    ActivationCode = Guid.NewGuid(),

                    // Bind Data
                    Id = Guid.NewGuid(),
                    Role = "User",
                    Status = "Active",
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    Email = model.Email,
                    IsEmailVerified = model.IsEmailVerified
                };

                db.Users.Add(newClientUser);
                db.SaveChanges();

                if (!newClientUser.IsEmailVerified)
                {
                    SendVerificaionLinkEmail(newClientUser.Email, newClientUser.ActivationCode.ToString());
                }

                return RedirectToAction("ClientUser", "AList");
            }

            return View(model);
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

            if (err != null)
            {
                ModelState.AddModelError("CoverImage", err);
            }

            if (ModelState.IsValid)
            {
                //Generate Id
                int m = (db.Events.Count()) + 1;
                string id = $"E{m}";

                int endIndex = model.YouTubeLink.Length;
                int startIndex = endIndex - 11;
                model.YouTubeLink = model.YouTubeLink.Substring(startIndex);

                var e = new Event
                {
                    Id = id,
                    UserId = model.UserId,
                    Title = model.Title,
                    View = 0,
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
            return View();
        }

        [HttpPost]
        public ActionResult CreateMerchandise(MerchandiseInsertVM model)
        {
            string err = ValidatePhoto(model.ImageURL);

            if (err != null)
            {
                ModelState.AddModelError("Image", err);
            }

            if (ModelState.IsValid)
            {
                string max = db.Merchandises.Max(m => m.Id) ?? "M000";
                int n = int.Parse(max.Substring(1));
                model.Id = (n + 1).ToString("'M'000");

                var merchandise = new Merchandise
                {
                    Id = model.Id,
                    Name = model.Name,
                    Image = SaveMerchandisePhoto(model.ImageURL),
                    Price = model.Price,
                    Status = model.Status
                };

                db.Merchandises.Add(merchandise);
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


        [NonAction]
        public bool IsEmailExist(string email)
        {
            //Bring DB to find the existing email
            using (db)
            {
                //FirstOrDefault() only return one record
                var v = db.Users.Where(s => s.Email == email).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificaionLinkEmail(string email, string activationCode)
        {
            //URL to activate code
            var verifyURL = "/UserAuth/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyURL);

            //Sender and receiver
            var fromEmail = new MailAddress("fundtasia1101@gmail.com", "Fundtasia Admin");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "lkwefecbllzpgmea"; // Replace with actual password (This password only can be use in Windows computer)
            string subject = "Fundtasia(Successfull to create an account)";

            string body = "<br><br>" +
                "<p>We are glad to tell you that your Fundtasia account is successfully created. Please click on the below link to verify your account</p>" +
                "<br><br>" +
                "<a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

    }
}