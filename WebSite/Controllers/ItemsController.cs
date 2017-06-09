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
using WebSite.Services;

namespace WebSite.Controllers
{
    [Authorize]
    public class ItemsController : BaseController
    {
        private JobService _jobService = new JobService();
        private static Dictionary<Guid, string> _selectedFeatures = new Dictionary<Guid, string>();
        private static string currentImage;
        private readonly string imagePattern = "data:image/gif;base64,{0}";

        private IEnumerable<ItemsViewModel> GetAll()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.DbContext.Accounts.First();
            ViewBag.Hottest = this.DbContext.Items.OrderBy(y => y.DueDateTime).ToList().Take(10);
            ViewBag.ByCategories = new Dictionary<string, IEnumerable<Item>>();
            this.DbContext.Categories.ForEach(x =>
            {
                ViewBag.ByCategories.Add(
                    x.Name,
                    this.DbContext.Items.OrderBy(y => y.DueDateTime).Where(y => y.CategoryId == x.Id).ToList());
            });

            ViewBag.Categories = this.DbContext.Categories.ToList();
            ViewBag.Items = this.DbContext.Items.Any() ? this.DbContext.Items.Select(x => new ItemsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                DueTo = x.DueDateTime,
                Image = x.Image,
                SellerId = x.SellerId,
                SellerName = x.SellerAccount.FirstName + " " + x.SellerAccount.LastName,
                SellerRating = x.SellerAccount.Rate.Value,
                UsersBet = x.Bets.FirstOrDefault(y => y.ItemId == x.Id) != null ? (decimal?)(x.Bets.FirstOrDefault(y => y.ItemId == x.Id).Amout) : null,
                HighestBet = x.Bets.ToList().Where(y => y.ItemId == x.Id).DefaultIfEmpty(new Bet()).Max(y => y.Amout)
            }).OrderByDescending(x => x.DueTo).ToList()
            : Enumerable.Empty<ItemsViewModel>();

            return ViewBag.Items;
        }

        //GET: Items
        public ActionResult Index()
        {
            GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Filter(FilterModel filterModel)
        {
            if (filterModel == null || filterModel.Categories == null)
            {
                ViewBag.Items = GetAll();
            }
            else
            {
                var ids = new List<Guid>();
                var items = this.DbContext.Items.Where(x => filterModel.Categories.Contains(x.CategoryId)).ToList();

                if (filterModel.Features != null && filterModel.Features.Any())
                {
                    var feature =
                        this.DbContext.Specifications.FirstOrDefault(x => filterModel.Features.Contains(x.SelectedValue));
                    if (feature != null)
                    {
                        ids.Add(feature.Id);
                    }

                    items = items.Where(x => ids.Contains(x.Id)).ToList();
                }

                ViewBag.Items = items.Select(x => new ItemsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    DueTo = x.DueDateTime,
                    Image = x.Image,
                    SellerId = x.SellerId,
                    SellerName = x.SellerAccount.FirstName + " " + x.SellerAccount.LastName,
                    SellerRating = x.SellerAccount.Rate != null ? (int?)x.SellerAccount.Rate.Value : null,
                    UsersBet = x.Bets.FirstOrDefault(y => y.ItemId == x.Id) != null ? (decimal?)(x.Bets.FirstOrDefault(y => y.ItemId == x.Id).Amout) : null,
                    HighestBet = x.Bets.ToList().Where(y => y.ItemId == x.Id).DefaultIfEmpty(new Bet()).Max(y => y.Amout)
                }).OrderByDescending(x => x.DueTo).ToList();
            }

            return PartialView("ItemsView");
        }

        //GET: Items/Details/5
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

        //GET: Items/Create
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
                    CategoryId = (byte)x.CategoryId,
                    Name = x.Name,
                    PosibleValues = x.PosibleValues.Split(',').ToList()
                }).ToList();

            return PartialView("Feature");
        }

        [HttpPost]
        public ActionResult GetFeatures(List<byte> categoryIds)
        {
            if (categoryIds == null)
            {
                return PartialView("FeatureFilter");
            }

            ViewBag.Features = new Dictionary<string, IEnumerable<FeatureViewModel>>();
            foreach (var categoryId in categoryIds)
            {
                ViewBag.Features.Add(
                    this.DbContext.Categories.ToList().First(x => x.Id == categoryId).Name,
                    this.DbContext.CategoryFeatures.ToList()
                        .Where(x => x.CategoryId == categoryId)
                        .Select(x => new FeatureViewModel
                        {
                            Id = x.Id,
                            CategoryId = (byte)x.CategoryId,
                            Name = x.Name,
                            PosibleValues = x.PosibleValues.Split(',').ToList()
                        }).ToList());
            }

            return PartialView("FeatureFilter");
        }

        [HttpPost]
        public HttpResponseMessage SetFeature(Guid featureId, string slectedValue)
        {
            _selectedFeatures.Add(featureId, slectedValue);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MakeABet(decimal amount, Guid itemId)
        {
            var id = Guid.NewGuid();

            this.DbContext.Bets.Add(new Bet
            {
                Id = id,
                Amout = amount,
                ItemId = itemId,
                BuyerId = new Guid(User.Identity.GetUserId())
            });

            var item = this.DbContext.Items.Find(itemId);
            item.HighestBetId = id;
            item.BuyerId = new Guid(User.Identity.GetUserId());

            this.DbContext.SaveChanges();

            return Redirect("Items\\Index");
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
            item.Image = this.GetImage();
            item.SellerId = new Guid(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                var itemId = Guid.NewGuid();
                item.Id = itemId;
                this.DbContext.Items.Add(item);
                _jobService.ScheduleAuctionEnd(item.DueDateTime, item.Id);

                _selectedFeatures.ForEach(x =>
                {
                    this.DbContext.Specifications.Add(new Specification
                    {
                        Id = Guid.NewGuid(),
                        SelectedValue = x.Value,
                        CategoryFeatureId = x.Key,
                        ItemId = itemId
                    });
                });

                this.DbContext.SaveChanges();
                currentImage = string.Empty;
                _selectedFeatures.Clear();
                return RedirectToAction("Index");
            }

            return this.Create();
        }

        //GET: Items/Edit/5
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
                    currentImage = string.Format(this.imagePattern, base64String);
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
                    return string.Format(this.imagePattern, base64String);
                }
            }
        }
        //POST: Items/Edit/5
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

        //GET: Items/Delete/5
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

        //POST: Items/Delete/5
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
