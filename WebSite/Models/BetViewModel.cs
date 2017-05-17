using System;
using System.Collections.Generic;

namespace WebSite.Models
{
    public class BetViewModel
    {
        public Guid UserId { get; set; }

        public List<Item> Items { get; set; }

        public List<Bet> UsersBets { get; set; }

        public List<HighestBetModel> HighestBets { get; set; }

        public List<ApplicationUser> Accounts { get; set; }
    }
}