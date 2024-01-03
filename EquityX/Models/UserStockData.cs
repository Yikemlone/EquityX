using System.ComponentModel.DataAnnotations;

namespace EquityX.Models
{
    /// <summary>
    /// This keeps track of the stocks the user has bought 
    /// </summary>
    public class UserStockData
    {
        [Key]
        public int ID { get; set; } // ID

        public int UserID { get; set; } // User's ID Foreign Key
        public int StockDataID { get; set; } // Stock's ID Foreign Key
        public decimal BuyInPrice { get; set;} // Price the stock was bought at
        public decimal? SellPrice { get; set; } // Price the stock was sold at
        public DateTime DateBought { get; set; } = DateTime.Now; // Date the stock was bought
        public DateTime? DateSold { get; set; } // Date the stock was sold
    }
}
