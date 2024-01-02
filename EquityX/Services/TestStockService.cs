//using EquityX.Models;

//namespace EquityX.Services
//{
//    /// <summary>
//    /// This is a test stock service, only returning test values.
//    /// </summary>
//    public class TestStockService : IStockService
//    {
//        public Task<StockData> GetStockData(string stockName)
//        {
//            return Task.FromResult(new StockData() 
//            { 
//                Name = "Apple",
//                BuyPrice = 5.5M,
//                SellPrice = 4.5M,
//                Currency = "USD", 
//                QuoteType = "Equity",
//                Symbol = "AAPL"
//            });
//        }

//        public Task<bool> BuyStock(StockData stock, decimal availableFunds)
//        {
//            return Task.FromResult(true);
//        }

//        public Task<decimal> SellStock(int stockID) // May need the user ID  as well, for validation
//        {
//            return Task.FromResult(100M);
//        }

//        public Task<List<StockData>> GetStockData()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<StockData>> GetUserStockData(List<string> stockNames)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
