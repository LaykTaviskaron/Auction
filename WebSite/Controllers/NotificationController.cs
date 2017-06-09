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
    [Authorize]
    public class NotificationController : BaseController
    {
        //GET: Notifications
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.DbContext.Accounts.Find(new Guid(currentUserId));

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
