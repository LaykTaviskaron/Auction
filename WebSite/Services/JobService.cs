using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using WebSite.Models;

namespace WebSite.Services
{
    public class JobService
    {

        public void ScheduleAuctionEnd(DateTime dueDate, Guid itemId)
        {
            var dueTime = dueDate.Subtract(DateTime.Now);
            Timer timer = new Timer(dueTime.TotalMilliseconds);
            timer.Elapsed += (sender, e) => NotifyAuctionEnd(itemId);
            timer.AutoReset = false;
            timer.Start();
        }

        private void NotifyAuctionEnd(Guid itemId)
        {
            var context = new AuctionEntities();

            var item = context.Items.Find(itemId);

            if (item.BuyerId != null)
            {
                context.Notifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    Message = "Auction has been finished, you've purchased the product!",
                    ReceiverId = item.BuyerId
                });
            }

            context.Notifications.Add(new Notification
            {
                Id = Guid.NewGuid(),
                Message = "Auction has been finished!",
                ReceiverId = item.SellerId
            });

            context.SaveChanges();
        }
    }
}