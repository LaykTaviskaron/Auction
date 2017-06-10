using System;

namespace WebSite.Models
{
    public class ItemsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public DateTime? DueTo { get; set; }

        public decimal? UsersBet { get; set; }

        public decimal? HighestBet { get; set; }

        public Guid SellerId { get; set; }

        public string SellerName { get; set; }

        public int? SellerRating { get; set; }

        public bool? IsAvailable { get; set; }

        public bool? IsReceived { get; set; }

        public bool? IsPayed { get; set; }
    }
}