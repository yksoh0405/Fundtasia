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

        //Sign Up Action
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        //Sign Up Post Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Exclude = "IsEmailVerified, ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                
                #region Email is already exist
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exist");
                    return View(user);
                }
                #endregion

                #region Generate activation code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.PasswordHash = Crypto.Hash(user.PasswordHash);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                //Bind the data
                user.IsEmailVerified = false;
                user.Role = "User";
                user.Status = "Active";

                #region Save to DB
                using (DBEntities1 da = new DBEntities1())
                {
                    da.Users.Add(user);
                    da.SaveChanges();

                    //Send email to user
                    SendVerificaionLinkEmail(user.Email, user.ActivationCode.ToString());
                    message = "You have complete to register an account in Fundtasia. The activation link has been sent to your email: " + user.Email;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalide Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (db)
            {
                db.Configuration.ValidateOnSaveEnabled = false;

                var v = db.Users.Where(s => s.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }


        //Login Action
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        //Login Post Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            //Verification
            using(DBEntities1 da = new DBEntities1())
            {
                var v = da.Users.Where(s => s.Email == login.Email).FirstOrDefault();
                if(v != null)
                {
                    //Force user to verify their email
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }

                    //Continue compare the password string
                    if (string.Compare(Crypto.Hash(login.PasswordHash), v.PasswordHash) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; //525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        Session["User"] = login;
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
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
            var verifyURL = "/Home/VerifyAccount/" + activationCode;
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

        [NonAction]
        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }
}