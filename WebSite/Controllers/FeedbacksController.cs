using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSite;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class FeedbacksController : BaseController
    {
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var feedback = new FeedbackModel
            {
                User = this.DbContext.Accounts.Find(id)
            };

            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Rate,Description,User")] FeedbackModel feedback)
        {
            if (ModelState.IsValid)
            {
                var user = this.DbContext.Accounts.Find(feedback.User.Id);

                feedback.Id = Guid.NewGuid();
                this.DbContext.Feedbacks.Add(new Feedback
                {
                    Id = feedback.Id,
                    Description = feedback.Description,
                    Rate = feedback.Rate,
                    Account = this.DbContext.Accounts.Find(user.Id),
                    UserId = feedback.User.Id
                });

                this.DbContext.SaveChanges();

                var rates = this.DbContext.Feedbacks.Where(x => x.UserId == user.Id).Select(x => (int)x.Rate);
                var average = rates.Average();
                user.Rate = (byte)average;

                this.DbContext.SaveChanges();
                return RedirectToAction("Index", "Items");
            }

            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = this.DbContext.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(this.DbContext.Accounts, "Id", "FirstName", feedback.UserId);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Rate,Description,UserId")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                this.DbContext.Entry(feedback).State = EntityState.Modified;
                this.DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(this.DbContext.Accounts, "Id", "FirstName", feedback.UserId);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = this.DbContext.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Feedback feedback = this.DbContext.Feedbacks.Find(id);
            this.DbContext.Feedbacks.Remove(feedback);
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
