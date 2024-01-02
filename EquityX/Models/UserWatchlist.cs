namespace EquityX.Models
{
    /// <summary>
    /// This keeps track the Stock data the user has added to their watchlist
    /// </summary>
    public class UserWatchlist
    {
        public int ID { get; set; } // ID
        public int UserID { get; set; } // User's ID
        public int StockDataID { get; set; } // Stock's name
        public DateTime DateAdded { get; set; } = DateTime.Now; // Date the stock was added to the watchlist
    }
}
