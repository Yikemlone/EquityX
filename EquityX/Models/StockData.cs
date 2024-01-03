using System.ComponentModel.DataAnnotations;

namespace EquityX.Models
{
    /// <summary>
    /// A model to contain basic information on stocks
    /// </summary>
    public class StockData
    {
        [Key]
        public int ID { get; set; } // Stock's ID

        public string Name { get; set; } // Stock's name
        public string Symbol { get; set; } // Stock's symbol
        public decimal BuyPrice { get; set; } // Stock's buy price
        public decimal SellPrice { get; set; } // Stock's sell price
        public string Currency { get; set; } // Stock's currency
        public string QuoteType { get; set; } // Stock's quote type
        public DateTime DateTime { get; set; } // Stock's date and time
    }
}
