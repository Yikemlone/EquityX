namespace EquityX.Models
{
    /// <summary>
    /// This keeps track of the stocks the user has bought at what price. 
    /// </summary>
    public class UserStockData
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int StockDataID { get; set; }
        public decimal BuyInPrice { get; set;}
        public DateTime DateBought { get; set; } = DateTime.Now;
    }
}
