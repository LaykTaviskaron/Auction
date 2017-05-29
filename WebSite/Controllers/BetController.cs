using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
            var highestBets =
                itemIds.Select(
                    x =>
                        new HighestBetModel
                        {
                            Id = (Guid)x,
                            Value = (decimal)this.DbContext.Bets.FirstOrDefault(y => y.Id == x)?.Amout
                        }).ToList();
            var items = new BetViewModel
            {
                UserId = userId,
                Items = this.DbContext.Items.Where(x => itemIds.Contains(x.Id)).ToList(),
                UsersBets = bets,
                HighestBets = highestBets,
                Accounts = this.DbContext.Accounts.ToList()
            };

            ViewData.Add(new KeyValuePair<string, object>("usersBets", items));
            return View();
        }

        // GET: Bet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
