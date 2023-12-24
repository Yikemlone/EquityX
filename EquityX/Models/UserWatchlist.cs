namespace EquityX.Models
{
    /// <summary>
    /// This keeps track the Stock data the user has added to their watchlist
    /// </summary>
    public class UserWatchlist
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int StockDataID { get; set; }
    }
}
