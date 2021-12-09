using Fundtasia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Fundtasia.Controllers
{
    public class AListController : Controller
    {
        //This controller is used to handle a list of record between Model and View
        DBEntities1 db = new DBEntities1();

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Staff(string sort = "Last Login Time", string sortdir = "DESC", int page = 1, string keyword = "")
        {
            keyword = keyword.Trim();

            //Sorting
            Func<User, object> fn = s => s.Id;

            switch (sort)
            {
                case "First Name": fn = s => s.FirstName; break;
                case "Last Name": fn = s => s.LastName; break;
                case "Verified": fn = s => s.IsEmailVerified; break;
                case "Status": fn = s => s.Status; break;
                case "Last Login Time": fn = s => s.LastLoginTime; break;
                case "Role": fn = s => s.Role; break;
            }

            var sorted = sortdir == "DESC" ? db.Users.Where(s => s.FirstName.Contains(keyword)).OrderByDescending(fn) : db.Users.Where(s => s.FirstName.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_Staff", model);

            return View(model);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult ClientUser(string sort = "Last Login Time", string sortdir = "DESC", int page = 1, string keyword = "")
        {
            keyword = keyword.Trim();

            //Sorting
            Func<User, object> fn = s => s.Id;

            switch (sort)
            {
                case "First Name": fn = s => s.FirstName; break;
                case "Last Name": fn = s => s.LastName; break;
                case "Verified": fn = s => s.IsEmailVerified; break;
                case "Status": fn = s => s.Status; break;
                case "Last Login Time": fn = s => s.LastLoginTime; break;
                case "Last Login IP": fn = s => s.LastLoginIP; break;
            }

            var sorted = sortdir == "DESC" ? db.Users.Where(s => s.FirstName.Contains(keyword)).OrderByDescending(fn) : db.Users.Where(s => s.FirstName.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_ClientUser", model);

            return View(model);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Event(string sort = "Created Date", string sortdir = "DESC", int page = 1, string keyword = "")
        {
            keyword = keyword.Trim();

            //Sorting
            Func<Event, object> fn = s => s.Id;

            switch (sort)
            {
                case "Id": fn = s => s.Id; break;
                case "Created Date": fn = s => s.CreatedDate; break;
                case "Title": fn = s => s.Title; break;
                case "Views": fn = s => s.View; break;
            }

            var sorted = sortdir == "DESC" ? db.Events.Where(s => s.Title.Contains(keyword)).OrderByDescending(fn) : db.Events.Where(s => s.Title.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_Event", model);
            
            return View(model);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Merchandise(string sort = "Id", string sortdir = "ASC", int page = 1, string keyword = "")
        {
            keyword = keyword.Trim();

            Func<Merchandise, object> fn = m => m.Id;
            switch (sort)
            {
                case "Id": fn = m => m.Id; break;
                case "Name": fn = m => m.Name; break;
                case "Price": fn = m => m.Price; break;
            }

            var sorted = sortdir == "DESC" ? db.Merchandises.Where(s => s.Name.Contains(keyword)).OrderByDescending(fn) : db.Merchandises.Where(s => s.Name.Contains(keyword)).OrderBy(fn);

            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            if (Request.IsAjaxRequest()) return PartialView("_Merchandise", model);

            return View(model);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Donation(string sort = "Time Donated", string sortdir = "DESC", int page = 1, string keyword = "")
        {
            Func<Donation, object> fn = d => d.Id;
            switch (sort)
            {
                case "Id": fn = d => d.Id; break;
                case "Donor": fn = d => d.User.FirstName; break;
                case "Time Donated": fn = d => d.TimeDonated; break;
                case "Amount": fn = d => d.Amount; break;
                case "Event": fn = d => d.EventId; break;
            }

            var sorted = sortdir == "DESC" ? db.Donations.Where(s => s.User.FirstName.Contains(keyword)).OrderByDescending(fn) : db.Donations.Where(s => s.User.FirstName.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_Donation", model);

            return View(model);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult MerchandiseSales(string sort = "Purchase Time", string sortdir = "DESC", int page = 1, string keyword = "")
        {
            keyword = keyword.Trim();

            //Sorting
            Func<UserMerchandise, object> fn = s => s.Id;

            switch (sort)
            {
                case "Purchase Time": fn = s => s.PurchaseTime; break;
                case "Name": fn = s => s.FullName; break;
                case "Merchandise Name": fn = s => s.Merchandise.Name; break;
                case "Size": fn = s => s.Size; break;
            }

            var sorted = sortdir == "DESC" ? db.UserMerchandises.Where(s => s.User.FirstName.Contains(keyword)).OrderByDescending(fn) : db.UserMerchandises.Where(s => s.User.FirstName.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_MerchandiseSales", model);

            return View(model);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Report(string sort = "Created Date", string sortdir = "DESC", int page = 1, string keyword = "")
        {
            keyword = keyword.Trim();

            //Sorting
            Func<Report, object> fn = s => s.Id;

            switch (sort)
            {
                case "Id": fn = s => s.Id; break;
                case "Title": fn = s => s.Title; break;
                case "Created Date": fn = s => s.CreatedDate; break;
                case "Created By": fn = s => s.User.LastName; break;
            }

            var sorted = sortdir == "DESC" ? db.Reports.Where(s => s.Title.Contains(keyword)).OrderByDescending(fn) : db.Reports.Where(s => s.Title.Contains(keyword)).OrderBy(fn);

            //Paging
            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var model = sorted.ToPagedList(page, 10);

            if (model == null)
            {
                return View();
            }

            if (page > model.PageCount)
            {
                return RedirectToAction(null, new { page = model.PageCount });
            }

            //Ajax Request
            if (Request.IsAjaxRequest()) return PartialView("_Report", model);

            return View(model);
        }
    }
}