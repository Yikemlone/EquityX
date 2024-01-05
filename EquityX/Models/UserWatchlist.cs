using System.ComponentModel.DataAnnotations;

namespace EquityX.Models
{
    /// <summary>
    /// This keeps track the Stock data the user has added to their watchlist
    /// </summary>
    public class UserWatchlist
    {
        [Key]
        public int ID { get; set; } 

        public int UserID { get; set; } // User's ID Foreign Key
        public string StockSymbol { get; set; } // Stock's symbol
        public DateTime DateAdded { get; set; } = DateTime.Now; // Date the stock was added to the watchlist
    }
}
