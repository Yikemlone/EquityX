namespace EquityX.Models
{
    /// <summary>
    /// This is keeps track of the users total value of their account and their watchlisted items. 
    /// </summary>
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal PortfolioValue { get; set; }
        public decimal AvailableFunds { get; set; } // Funds that can be used to buy stocks
        public List<UserStockData> UserStocks { get; set; }
        public List<UserWatchlist> UserWatchlist { get; set; }
    }
}

