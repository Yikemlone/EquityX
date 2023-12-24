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
        /// Will call an API to retrive stock data with a search for its name
        /// </summary>
        /// <param name="stockName"></param>
        /// <returns>Stock data</returns>
        public Task<StockData> GetStockData(string stockName);

        /// <summary>
        /// Will pass in the current stock data. Using that data object, it will compare avaiable funds with
        /// the current price of the stock to enusre that can buy it.
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="availableFunds"></param>
        /// <returns>Returns a boolean value if the purchase was succesful</returns>
        public Task<bool> BuyStock(StockData stock, decimal availableFunds);

        /// <summary>
        /// This will sell the stock the user purchaed at a cetain buy in price
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns>Value of the stock after calculating loss and gain</returns>
        public Task<decimal> SellStock(int stockID);
    }
}
