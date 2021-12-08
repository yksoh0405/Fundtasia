using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net.Mail;
using System.Net;
using Fundtasia.Models;
using System.Net.Sockets;
using System.Data.Entity.Validation;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace Fundtasia.Controllers
{
    public class UserAuthController : Controller
    {
        //This controller is used to handle the user authentication
        DBEntities1 db = new DBEntities1();

        //Sign Up Action
        [HttpGet]
        public ActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                //The user cannot come to signup page after login
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //Sign Up Post Action
        [HttpPost]
        public ActionResult SignUp(SignUpVM model)
        {
            if (User.Identity.IsAuthenticated)
            {
                //The user cannot come to signup page after login
                return RedirectToAction("Index", "Home");
            }
            bool Status = false;
            string message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                #region Check confirm password == password
                if (model.PasswordHash != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Confirm Password and Password does not match");
                    return View(model);
                }
                #endregion

                #region Email is already exist
                if (IsEmailExist(model.Email))
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exist");
                    return View(model);
                }
                #endregion

                #region Save to DB
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    Role = "User",
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PasswordHash = Crypto.Hash(model.PasswordHash),
                    IsEmailVerified = false,
                    ActivationCode = Guid.NewGuid(),
                    Status = "Active"
                };
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
            return View(model);
        }

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

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
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //Login Post Action
        [HttpPost]
        public ActionResult LogIn(UserLogin login, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(s => s.Email == login.Email).FirstOrDefault();

                if (user != null && VerifyPassword(user.PasswordHash, login.PasswordHash))
                {
                    user.LastLoginTime = DateTime.Now;
                    user.LastLoginIP = GetLocalIPAddress();
                    db.SaveChanges();
                    SignIn(user.Id.ToString(), user.Role, login.RememberMe);
                    Session["UserSession"] = new User(user.Id, user.Email, user.Role, user.FirstName, user.LastName, user.Status, (DateTime)user.LastLoginTime, user.LastLoginIP);

                    if (ReturnUrl == "")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "Email and password not matched");
                }
            }
            return View(login);
        }

        //Log Out
        public ActionResult Logout()
        {
            //Clear the session
            Request.GetOwinContext().Authentication.SignOut();
            Session.Remove("UserSession");
            return RedirectToAction("LogIn", "UserAuth");
        }

        private void SignIn(string id, string role, bool rememberMe)
        {
            var iden = new ClaimsIdentity("AUTH");

            iden.AddClaim(new Claim(ClaimTypes.Name, id));
            iden.AddClaim(new Claim(ClaimTypes.Role, role));

            var prop = new AuthenticationProperties
            {
                IsPersistent = rememberMe
            };

            Request.GetOwinContext().Authentication.SignIn(prop, iden);
        }

        private bool VerifyPassword(string hash, string password)
        {
            bool status = false;
            password = Crypto.Hash(password);
            status = String.Compare(hash, password) == 0 ? true : false;
            return status;
        }

        public ActionResult ForgotPassword()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordVM fgtPwdVM)
        {
            Guid resetCode = Guid.NewGuid();
            var verifyUrl = "/UserAuth/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            if (ModelState.IsValid)
            {
                if (IsEmailExist(fgtPwdVM.Email))
                {
                    var model = db.Users.Where(s => s.Email == fgtPwdVM.Email).FirstOrDefault();

                    // Add the verification time
                    var current = DateTime.Now.AddMinutes(5);

                    var fgtPwd = new PasswordReset
                    {
                        UserId = model.Id,
                        Code = resetCode,
                        TimeOver = current
                    };

                    db.PasswordResets.Add(fgtPwd);
                    db.SaveChanges();

                    var subject = "Password Reset Request";
                    var body = "Hi " + model.FirstName + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +
                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                    SendEmail(model.Email, body, subject);

                    ViewBag.Message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }

            return View();
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("fundtasia1101@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("fundtasia1101@gmail.com", "lkwefecbllzpgmea");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);

            }
        }

        //public ActionResult ResetPassword(string id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }



        //    return View();
        //}

        [Authorize(Roles = "User")]
        public ActionResult ViewProfile()
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