using System;

namespace WebSite.Models
{
    public class NotificationViewModel
    {
        public System.Guid Id { get; set; }
        public string Message { get; set; }
        public Guid? ItemId { get; set; }
        public Guid? ReceiverId { get; set; }
        public byte? NotificationTypeId { get; set; }

        public virtual Account Account { get; set; }
        public virtual NotificcationType NotificcationType { get; set; }
        public virtual Item Item { get; set; }
    }
}