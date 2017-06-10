using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    [Authorize]
    public class BetController : BaseController
    {
        // GET: Bet
        public ActionResult Index()
        {
            var userId = new Guid(User.Identity.GetUserId());
            ViewBag.CurrentUserId = userId;
            var bets = this.DbContext.Bets.Where(x => x.BuyerId == userId).ToList();
            var itemIds = bets.Select(x => x.ItemId).ToList();
            var items = this.DbContext.Items.ToList()
                .Where(x => itemIds.Contains(x.Id))
                .Select(x => new BetViewModel
                {
                    Item = x as Item,
                    UsersBet = bets.FirstOrDefault(y => y.ItemId == x.Id) as Bet,
                    HighestBet = x.Bets.FirstOrDefault(y => y.Id == x.HighestBetId) as Bet,
                    Account = x.SellerAccount as Account
                }).ToList();

            ViewBag.UserBets = items;
            return View();
        }

        [HttpPost]
        public HttpStatusCodeResult ConfirmReceived(Guid itemId)
        {
            var item = this.DbContext.Items.Find(itemId);
            item.IsReceived = true;
            this.DbContext.Notifications.Add(new Notification
            {
                Id = Guid.NewGuid(),
                ItemId = itemId,
                ReceiverId = item.SellerId,
                Message = $"Your item {item.Name} has been received."
            });
            this.DbContext.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        // GET: Bet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
