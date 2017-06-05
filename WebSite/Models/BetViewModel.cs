namespace WebSite.Models
{
    public class BetViewModel
    {
        public Item Item { get; set; }

        public Bet UsersBet { get; set; }

        public Bet HighestBet { get; set; }

        public Account Account { get; set; }
    }
}