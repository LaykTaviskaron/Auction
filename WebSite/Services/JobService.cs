using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using WebSite.Models;

namespace WebSite.Services
{
    public class JobService : IJobService
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
            item.IsAvailable = false;

            if (item.BuyerId != null)
            {
                var sellerEmail = context.Accounts.Find(item.SellerId).Email;
                context.Notifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    Message = "Auction has been finished, you've purchased the product!\nPlease contact seller via email: " + sellerEmail,
                    ReceiverId = item.BuyerId.Value
                });
            }

            context.Notifications.Add(new Notification
            {
                Id = Guid.NewGuid(),
                Message = "Auction has been finished!\nPlease contact buyer via email: " + item.BuyerAccount.Email,
                ReceiverId = item.SellerId
            });

            context.SaveChanges();
        }
    }
}