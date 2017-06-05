using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class BetController : BaseController
    {
        // GET: Bet
        public ActionResult Index()
        {
            var userId = new Guid(User.Identity.GetUserId());
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

        // GET: Bet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
