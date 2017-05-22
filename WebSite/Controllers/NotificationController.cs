using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class NotificationController : BaseController
    {
        //GET: Notifications
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.DbContext.Accounts.Where(x => x.Email == "artem.sokolov@sigma.software").FirstOrDefault();
            //var currentUser = this.DbContext.Accounts.Where(x => x.Email == "olena.slukhaievska@sigma.software").FirstOrDefault();

            //this.DbContext.Notifications.Add(new Notification
            //{
            //    Account = currentUser,
            //    Id = Guid.NewGuid(),
            //    ReceiverId = currentUser?.Id,
            //    Message = "Welcome to your account"
            //});

            this.DbContext.SaveChanges();

            ViewBag.Notifications = this.DbContext.Notifications.Select(x => new NotificationViewModel
            {
                Id = x.Id,
                Message = x.Message
            }).ToList();

            return View(ViewBag.Notifications);
        }

        //GET: Notifications/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = this.DbContext.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        //GET: Notifications/Create
        public ActionResult Create()
        {
            ViewBag.BuyerId = new SelectList(this.DbContext.Accounts, "Id", "FirstName");
            ViewBag.SellerId = new SelectList(this.DbContext.Accounts, "Id", "FirstName");
            ViewBag.HighestBetId = new SelectList(this.DbContext.Bets, "Id", "Id");
            ViewBag.CategoryId = new SelectList(this.DbContext.Categories, "Id", "Name");
            ViewBag.FeatureId = new SelectList(this.DbContext.Specifications, "Id", "");

            return View();
        }

        //GET: Notifications/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = this.DbContext.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        //POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Notification notification = this.DbContext.Notifications.Find(id);
            this.DbContext.Notifications.Remove(notification);
            this.DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
