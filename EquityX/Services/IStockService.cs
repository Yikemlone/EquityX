using EquityX.Models;

namespace EquityX.Services
{
    /// <summary>
    /// Defining the interface for the StockService. This Service will return stock data
    /// and allow for the purchase of assets
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// Will call an API to retrieve stock data for the most popular stocks
        /// </summary>
        /// <returns>List<StockData></returns>
        public Task<List<StockData>> GetStockData();

        /// <summary>
        /// Will call an API to retrieve stock data by symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>StockData</returns>
        public Task<StockData> GetStockData(string symbol);

        /// <summary>   
        /// Get the stock data for the stocks the user owns
        /// </summary>
        /// <returns>List<StockData></returns>
        public Task<List<StockData>> GetUserStockData(int userID);

        /// <summary>   
        /// Get the stock data for the stocks the user is tracking via the watchlist
        /// </summary>
        /// <returns>List<StockData></returns>
        public Task<List<StockData>> GetUserWatchlistData(int userID);

        /// <summary>
        /// Will pass in the current stock data. Using that data object, it will compare available funds with
        /// the current price of the stock to ensure that the user can buy it.
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="availableFunds"></param>
        /// <returns>bool</returns>
        public Task<bool> BuyStock(StockData stock, int userID);

        /// <summary>
        /// This will sell the stock the user purchased at a certain buy-in price
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns>decimal</returns>
        public Task<decimal> SellStock(UserStockData userStock, int userID);
    }
}
