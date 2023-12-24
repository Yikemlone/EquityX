namespace EquityX.Models
{
    /// <summary>
    /// A model to contain basic information on stocks
    /// </summary>
    public class StockData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public string Currency { get; set; }
        public string QuoteType { get; set; }
    }
}
