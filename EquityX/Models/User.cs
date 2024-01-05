using System.ComponentModel.DataAnnotations;

namespace EquityX.Models
{
    /// <summary>
    /// This is keeps track of the users total value of their account and their watchlisted items. 
    /// </summary>
    public class User
    {
        [Key]
        public int ID { get; set; } // User's ID

        public string Name { get; set; } // User's name
        public string Username { get; set; }
        public string Password { get; set; } // User's password
        public string Hash { get; set; } // User's hash
        public decimal PortfolioValue { get; set; } // Total value of all stocks owned and available funds
        public decimal AvailableFunds { get; set; } // Funds that can be used to buy stocks
        public List<UserStockData> UserStocks { get; set; } = new(); // List of stocks the user owns
        public List<UserWatchlist> UserWatchlist { get; set; } = new(); // List of stocks the user is watching
    }
}

