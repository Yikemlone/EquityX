using EquityX.Context;
using EquityX.DTO;
using EquityX.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace EquityX.Services
{
    public class StockService : IStockService
    {
        private HttpClient _client;
        private string API_KEY = "7OuBqBEphu85XcIXzDamU1aClxRlGZUr9q1GYaYY"; // TODO: Move to config file or out of code
        private string URL = "https://yfapi.net";
        private readonly EquityXDbContext _context;

        public StockService(HttpClient httpClient, EquityXDbContext context)
        {
            _client = httpClient;
            _context = context;
        }

        public async Task<bool> BuyStock(StockData stock, int userID)
        {
            // Get the user's data
            var user = await _context.Users
                .Where(u => u.ID == userID)
                .Select(e => e)
                .FirstOrDefaultAsync();

            // Check if the user has enough funds to buy the stock
            if (user.AvailableFunds < stock.BuyPrice)
            {
                return false;
            }

            // Update the user's available funds and portfolio value
            user.AvailableFunds -= stock.BuyPrice;
            user.PortfolioValue += stock.SellPrice;

            // Add the stock to the user's portfolio
            await _context.UserStockData.AddAsync(new UserStockData()
            {
                UserID = userID,
                StockSymbol = stock.Symbol,
                BuyInPrice = stock.BuyPrice,
                DateBought = DateTime.Now,
            });

            var rowsEffected = await _context.SaveChangesAsync();

            if (rowsEffected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<decimal> SellStock(UserStockData userStock, int userID)
        {
            User user = await _context.Users
                .Where(u => u.ID == userID)
                .Select(e => e)
                .FirstOrDefaultAsync();

            // Get the stock data
            StockData currecntStockData = await GetStockDataBySymbol(userStock.StockSymbol);

            // Selling quantity could be a problem later
            userStock.SellPrice = currecntStockData.SellPrice;
            userStock.DateSold = DateTime.Now; 

            // Update the user's available funds and portfolio value
            user.AvailableFunds += currecntStockData.SellPrice;
            user.PortfolioValue -= currecntStockData.SellPrice;

            int rowsEffected = await _context.SaveChangesAsync();

            if (rowsEffected == 0)
            {
                return 0;
            }

            return currecntStockData.SellPrice;
        }

        public async Task<List<StockData>> GetStockData()
        {
            List<StockData> stockDataList = new List<StockData>();

            try
            {
                string symbols = await GetTrendingStockData();

                // Create the request with the API key header
                Uri stockDetailsUri = 
                    new Uri(string.Format(URL + $"/v6/finance/quote?region=US&lang=en&symbols={symbols}", string.Empty));
                
                var request = new HttpRequestMessage(HttpMethod.Get, stockDetailsUri);
                request.Headers.Add("X-API-KEY", API_KEY);

                // Send the request to the server
                var task = _client.SendAsync(request);
                var response = task.Result;

                // Check that the response is successful or throw an exception
                string responsebody = "";
                var message = response.EnsureSuccessStatusCode();

                if (!message.IsSuccessStatusCode)
                    throw new Exception("Error getting stock data.");

                responsebody = response.Content.ReadAsStringAsync().Result;
                QuoteDTO stockDataResponse = JsonConvert.DeserializeObject<QuoteDTO>(responsebody);
                
                foreach (var res in stockDataResponse.QuoteResponse.Result)
                {
                    stockDataList.Add(new StockData()
                    {
                        Name = res.longName,
                        BuyPrice = res.bid,
                        SellPrice = res.ask,
                        Currency = res.currency,
                        QuoteType = res.quoteType,
                        Symbol = res.symbol
                    });
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return stockDataList;
        }

        public Task<List<StockData>> GetStockData(string symbols)
        {
            List<StockData> stockDataList = new List<StockData>();

            try
            {
                // Create the request with the API key header
                Uri stockDetailsUri =
                    new Uri(string.Format(URL + $"/v6/finance/quote?region=US&lang=en&symbols={symbols}", string.Empty));

                var request = new HttpRequestMessage(HttpMethod.Get, stockDetailsUri);
                request.Headers.Add("X-API-KEY", API_KEY);

                // Send the request to the server
                var task = _client.SendAsync(request);
                var response = task.Result;

                // Check that the response is successful or throw an exception
                string responsebody = "";
                var message = response.EnsureSuccessStatusCode();

                if (!message.IsSuccessStatusCode)
                    throw new Exception("Error getting stock data.");

                responsebody = response.Content.ReadAsStringAsync().Result;
                QuoteDTO stockDataResponse = JsonConvert.DeserializeObject<QuoteDTO>(responsebody);

                foreach (var res in stockDataResponse.QuoteResponse.Result)
                {
                    stockDataList.Add(new StockData()
                    {
                        Name = res.longName,
                        BuyPrice = res.bid,
                        SellPrice = res.ask,
                        Currency = res.currency,
                        QuoteType = res.quoteType,
                        Symbol = res.symbol
                    });
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return Task.FromResult(stockDataList);
        }

        public async Task<List<StockData>> GetStockData(int userID)
        {
            // Grab the user's stock data
            List<UserStockData> userStockData = await _context.UserStockData
                .Where(u => u.UserID == userID && u.DateSold == null)
                .Select(e => e)
                .ToListAsync();

            if (userStockData.Count == 0)
            {
                return new List<StockData>();
            }

            // Making a string of the symbols to pass to the API
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var stock in userStockData)
            {
                stringBuilder.Append(stock.StockSymbol + ",");
            }

            // Remove the last comma
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            // Get the stock data from the API
            List<StockData> stockData = await GetStockData(stringBuilder.ToString());

            return stockData;
        }

        public async Task<List<UserStockData>> GetUserStockData(int userID)
        {
            // Grab the user's stock data
            List<UserStockData> userStockData = await _context.UserStockData
                .Where(u => u.UserID == userID)
                .Select(e => e)
                .ToListAsync();
            
            if (userStockData.Count == 0)
            {
                return new List<UserStockData>();
            }

            return userStockData;
        }

        public async Task<List<StockData>> GetUserWatchlistData(int userID)
        {
            // Grab the user's stock data
            List<UserWatchlist> userWatchlist = await _context.UserWatchlist
                .Where(u => u.UserID == userID)
                .Select(e => e)
                .ToListAsync();

            if (userWatchlist.Count == 0)
            {
                return new List<StockData>();
            }

            // Making a string of the symbols to pass to the API
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var stock in userWatchlist)
            {
                stringBuilder.Append(stock.StockSymbol + ",");
            }

            // Remove the last comma
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            // Get the stock data from the API
            List<StockData> stockData = await GetStockData(stringBuilder.ToString());

            return stockData;
        }
     
        public Task<string> GetTrendingStockData()
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                // Create the request with the API key header
                Uri trendingStocksUri = new Uri(string.Format(URL + "/v1/finance/trending/US", string.Empty));
                var trendingRequest = new HttpRequestMessage(HttpMethod.Get, trendingStocksUri);
                trendingRequest.Headers.Add("X-API-KEY", API_KEY);

                // Send the request to the server
                var trendingTask = _client.SendAsync(trendingRequest);
                var trendingResponse = trendingTask.Result;

                // Check that the response is successful or throw an exception
                string trendingResponsebody = "";
                var trendingMessage = trendingResponse.EnsureSuccessStatusCode();

                if (!trendingMessage.IsSuccessStatusCode) 
                    throw new Exception("Error getting stock data.");

                trendingResponsebody = trendingResponse.Content.ReadAsStringAsync().Result;
                TrendingDTO trendingStocks = JsonConvert.DeserializeObject<TrendingDTO>(trendingResponsebody);
                List<TrendingQuote> quotes = trendingStocks.Finance.Result[0].Quotes;

                foreach (var quote in quotes)
                {
                    stringBuilder.Append(quote.Symbol + ",");
                }

                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return Task.FromResult(stringBuilder.ToString());
        }

        public Task<StockData> GetStockDataBySymbol(string symbol)
        {
            StockData stockData = new StockData();
            Uri uri = new Uri(string.Format(URL + $"/v6/finance/quote?region=US&lang=en&symbols={symbol}", string.Empty));

            try
            {
                // Create the request with the API key header
                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                request.Headers.Add("X-API-KEY", API_KEY);

                // Send the request to the server
                var task = _client.SendAsync(request);
                var response = task.Result;

                // Check that the response is successful or throw an exception
                string responsebody = "";
                var message = response.EnsureSuccessStatusCode();

                if (message.IsSuccessStatusCode)
                {
                    responsebody = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception("Error getting stock data.");
                }

                // Deserialize the JSON response
                QuoteDTO quoteDTO = JsonConvert.DeserializeObject<QuoteDTO>(responsebody);

                stockData.Name = quoteDTO.QuoteResponse.Result[0].longName;
                stockData.BuyPrice = quoteDTO.QuoteResponse.Result[0].bid;
                stockData.SellPrice = quoteDTO.QuoteResponse.Result[0].ask;
                stockData.Currency = quoteDTO.QuoteResponse.Result[0].currency;
                stockData.QuoteType = quoteDTO.QuoteResponse.Result[0].quoteType;
                stockData.Symbol = quoteDTO.QuoteResponse.Result[0].symbol;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return Task.FromResult(stockData);
        }

        public async Task<decimal> CalulatePortfolioValue(int userID)
        {
            decimal portfolioValue = 0;
            
            User user = await _context.Users
                .Where(u => u.ID == userID)
                .Select(e => e)
                .FirstOrDefaultAsync();

            List<StockData> stockData = await GetStockData(userID);

            foreach (var stock in stockData)
            {
                portfolioValue += stock.SellPrice;
            }

            portfolioValue += user.AvailableFunds;
            user.PortfolioValue = portfolioValue;

            await _context.SaveChangesAsync();

            return portfolioValue;
        }

        public async Task<bool> AddToWatchlist(StockData stockData, int userID)
        {
            User user = await _context.Users
                .Where(u => u.ID == userID)
                .Select(e => e)
                .FirstOrDefaultAsync();

            await _context.UserWatchlist.AddAsync(new UserWatchlist()
            {
                DateAdded = DateTime.Now,
                StockSymbol = stockData.Symbol,
                UserID = userID,
            });


            int rowsEffected = await _context.SaveChangesAsync();

            if(rowsEffected == 0)
            {
                return false;
            }   

            return true;
        }

        public async Task<bool> RemoveFromWatchlist(UserWatchlist userWatchlist, int userID)
        {
            User user = await _context.Users
                .Where(u => u.ID == userID)
                .Select(e => e)
                .FirstOrDefaultAsync();

            _context.UserWatchlist.Remove(userWatchlist);

            int rowsEffected = await _context.SaveChangesAsync();

            if(rowsEffected == 0)
            {
                return false;
            }   

            return true;
        }
    }
}
