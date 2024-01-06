using EquityX.Context;
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
        private string API_KEY = "ecEqpK0r3B2y6CWbr29Fq1DVnZz83IAq8LVzDUDa"; // TODO: Move to config file or out of code
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

            // Update the user's available funds
            user.AvailableFunds -= stock.BuyPrice;

            // Add the stock to the user's portfolio
            await _context.UserStockData.AddAsync(new UserStockData()
            {
                UserID = userID,
                StockSymbol = stock.Symbol,
                BuyInPrice = stock.BuyPrice,
                SellPrice = stock.SellPrice,
                DateBought = DateTime.Now,
            });

            var rowsEffected = await _context.SaveChangesAsync();

            if (rowsEffected == 0)
            {
                return false;
            }

            return true;
        }

        // TODO: Returns the value the stock sold for after calculating loss and gain
        public async Task<decimal> SellStock(UserStockData userStock, int userID)
        {
            User user = await _context.Users
                .Where(u => u.ID == userID)
                .Select(e => e)
                .FirstOrDefaultAsync();

            // Get the stock data
            StockData currecntStockData = await GetStockData(userStock.StockSymbol);

            // Selling quantity could be a problem later
            userStock.SellPrice = currecntStockData.SellPrice;
            userStock.DateSold = DateTime.Now; 
            user.AvailableFunds += currecntStockData.SellPrice;

            int rowsEffected = await _context.SaveChangesAsync();

            return currecntStockData.SellPrice;
        }

        // TODO: This should return a list of the popular stocks on the market, however there is an issue 
        // with setting up classes to deserialize the JSON response due to the way the response is structured
        public Task<List<StockData>> GetStockData()
        {
            List<StockData> stockDataList = new List<StockData>();
            Uri trendingStocksUri = new Uri(string.Format(URL + "/v1/finance/trending/US", string.Empty));

            try
            {
                // TODO: Grab populare stocks from the API

                // Create the request with the API key header
                var trendingRequest = new HttpRequestMessage(HttpMethod.Get, trendingStocksUri);
                trendingRequest.Headers.Add("X-API-KEY", API_KEY);

                // Send the request to the server
                var trendingTask = _client.SendAsync(trendingRequest);
                var trendingResponse = trendingTask.Result;

                // Check that the response is successful or throw an exception
                string trendingResponsebody = "";
                var trendingMessage = trendingResponse.EnsureSuccessStatusCode();

                if (trendingMessage.IsSuccessStatusCode)
                {
                    trendingResponsebody = trendingResponse.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception("Error getting stock data.");
                }

                APIResponse.FinanceResponse.Root trendingStocks = 
                    JsonConvert.DeserializeObject<APIResponse.FinanceResponse.Root>(trendingResponsebody);

                // TODO: Symbols from the first request to then make the second request
                // grabbing all the data for the trending stocks


                // Making a string of the symbols to pass to the API
                StringBuilder stringBuilder = new StringBuilder();

                var quotes = trendingStocks.finance.result[0].quotes;

                for (int i = 0; i < 10; i++) 
                {
                    stringBuilder.Append(quotes[i].symbol + ",");
                }

                // Remove the last comma
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                
                string symbols = stringBuilder.ToString();
                
                Uri stockDetailsUri = 
                    new Uri(string.Format(URL + $"/v6/finance/quote?region=US&lang=en&symbols={symbols}", string.Empty));

                // Create the request with the API key header
                var request = new HttpRequestMessage(HttpMethod.Get, stockDetailsUri);
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
                APIResponse.QuoteResponse.Root myDeserializedClass = JsonConvert.DeserializeObject<APIResponse.QuoteResponse.Root>(responsebody);
                
                foreach (var res in myDeserializedClass.quoteResponse.result)
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

        public Task<StockData> GetStockData(string symbol)
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
                APIResponse.QuoteResponse.Root myDeserializedClass = JsonConvert.DeserializeObject<APIResponse.QuoteResponse.Root>(responsebody);

                stockData.Name = myDeserializedClass.quoteResponse.result[0].longName;
                stockData.BuyPrice = myDeserializedClass.quoteResponse.result[0].bid;
                stockData.SellPrice = myDeserializedClass.quoteResponse.result[0].ask;
                stockData.Currency = myDeserializedClass.quoteResponse.result[0].currency;
                stockData.QuoteType = myDeserializedClass.quoteResponse.result[0].quoteType;
                stockData.Symbol = myDeserializedClass.quoteResponse.result[0].symbol;
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return Task.FromResult(stockData);
        }

        // TODO: This would be connected to the database and would return the stock
        // data for the stocks the user owns/is tracking via the watchlist
        public async Task<List<StockData>> GetUserStockData(int userID)
        {
            // Grab the user's stock data
            List<UserStockData> userStockData = await _context.UserStockData
                .Where(u => u.UserID == userID)
                .Select(e => e)
                .ToListAsync();

            // Making a string of the symbols to pass to the API
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var stock in userStockData)
            {
                stringBuilder.Append(stock.StockSymbol + ",");
            }

            // Remove the last comma
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            // Get the stock data from the API




            throw new NotImplementedException();
        }

        public Task<List<StockData>> GetUserWatchlistData(int userID)
        {
            throw new NotImplementedException();
        }
    }
}
