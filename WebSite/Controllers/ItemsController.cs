﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using WebGrease.Css.Extensions;
using WebSite;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class ItemsController : BaseController
    {
        private Dictionary<Guid, string> selectedFeatures = new Dictionary<Guid, string>();
        private static string currentImage;

        // GET: Items
        public ActionResult Index()
        {
            ViewData = new ViewDataDictionary()
            {
                {"Suggested", this.DbContext.Items.ToList()}
            };
            var items = this.DbContext.Items;
            return View(items);
        }

        // GET: Items/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = this.DbContext.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.BuyerId = new SelectList(this.DbContext.Accounts, "Id", "FirstName");
            ViewBag.SellerId = new SelectList(this.DbContext.Accounts, "Id", "FirstName");
            ViewBag.HighestBetId = new SelectList(this.DbContext.Bets, "Id", "Id");
            ViewBag.CategoryId = new SelectList(this.DbContext.Categories, "Id", "Name");
            ViewBag.FeatureId = new SelectList(this.DbContext.Specifications, "Id", "");

            return View();
        }

        [HttpGet]
        public ActionResult GetFeatures(byte categoryId)
        {
            ViewBag.Features = this.DbContext.CategoryFeatures.ToList()
                .Where(x => x.CategoryId == categoryId)
                .Select(x => new FeatureViewModel
                {
                    Id = x.Id,
                    CategoryId = (byte) x.CategoryId,
                    Name = x.Name,
                    PosibleValues = x.PosibleValues.Split(',').ToList()
                }).ToList();

            return PartialView("Feature");
        }

        [HttpPost]
        public HttpResponseMessage SetFeature(Guid featureId, string slectedValue)
        {
            this.selectedFeatures.Add(featureId, slectedValue);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,Name,Description,DueDateTime,IsAvailable,CategoryId,FeatureId,MinBet")] Item item)
        {
            item.IsPayed = false;
            item.IsReceived = false;
            item.BuyerId = null;
            item.HighestBetId = null;

            //Get user ID from context
            //item.SellerId = 

            if (ModelState.IsValid)
            {
                var itemId = Guid.NewGuid();
                item.Id = itemId;
                this.DbContext.Items.Add(item);

                this.selectedFeatures.ForEach(x =>
                {
                    this.DbContext.Specifications.Add(new Specification
                    {
                        Id = new Guid(),
                        SelectedValue = x.Value,
                        CategoryFeatureId = x.Key,
                        ItemId = itemId
                    });
                });

                this.DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return this.Create();
        }

        // GET: Items/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = this.DbContext.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuyerId = new SelectList(this.DbContext.Accounts, "Id", "FirstName", item.BuyerId);
            ViewBag.SellerId = new SelectList(this.DbContext.Accounts, "Id", "FirstName", item.SellerId);
            ViewBag.HighestBetId = new SelectList(this.DbContext.Bets, "Id", "Id", item.HighestBetId);
            ViewBag.CategoryId = new SelectList(this.DbContext.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }

        [HttpPost]
        public HttpResponseMessage ImageUpload()
        {
            try
            {
                var file = System.Web.HttpContext.Current.Request.Files["ItemImage"];
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();

                    string base64String = Convert.ToBase64String(array);
                    currentImage = $"data:image/gif;base64,{base64String}";
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public string GetImage()
        {
            return string.IsNullOrEmpty(currentImage) ? this.GetDefaultImage() : currentImage;
        }

        private string GetDefaultImage()
        {
            using (Image image = Image.FromFile(Server.MapPath("~/Content/Images/default-picture.png")))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    string base64String = Convert.ToBase64String(imageBytes);
                    return $"data:image/gif;base64,{base64String}";
                }
            }
        }
        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,DueDateTime,IsAvailable,SellerId,CategoryId,FeatureId,MinBet,IsPayed,IsReceived,BuyerId,HighestBetId")] Item item)
        {
            if (ModelState.IsValid)
            {
                this.DbContext.Entry(item).State = EntityState.Modified;
                this.DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuyerId = new SelectList(this.DbContext.Accounts, "Id", "FirstName", item.BuyerId);
            ViewBag.SellerId = new SelectList(this.DbContext.Accounts, "Id", "FirstName", item.SellerId);
            ViewBag.HighestBetId = new SelectList(this.DbContext.Bets, "Id", "Id", item.HighestBetId);
            ViewBag.CategoryId = new SelectList(this.DbContext.Categories, "Id", "Name", item.CategoryId);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = this.DbContext.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Item item = this.DbContext.Items.Find(id);
            this.DbContext.Items.Remove(item);
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
