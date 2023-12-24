using EquityX.Models;

namespace EquityX.ViewModels
{
    public class HomeViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal PortfolioValue { get; set; }
        public decimal AvailableFunds { get; set; }
        public List<UserStockData> UserStocks { get; set; }
        public List<UserWatchlist> UserWatchlist { get; set; }
    }
}
