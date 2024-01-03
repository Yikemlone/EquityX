using EquityX.APIResponse;
using EquityX.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EquityX.Services
{
    public class StockService : IStockService
    {
        private HttpClient _client;
        private string API_KEY = "ecEqpK0r3B2y6CWbr29Fq1DVnZz83IAq8LVzDUDa"; // TODO: Move to config file or out of code
        private string URL = "https://yfapi.net";

        public StockService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public Task<bool> BuyStock(StockData stock, decimal availableFunds)
        {
            // Check if the user has enough funds to buy the stock
            if (availableFunds < stock.BuyPrice)
            { 
                Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        // TODO: This should return a list of the popular stocks on the market, however there is an issue 
        // with setting up classes to deserialize the JSON response due to the way the response is structured
        public Task<List<StockData>> GetStockData()
        {
            List<StockData> stockDataList = new List<StockData>();
            Uri uri = new Uri(string.Format(URL + "/v6/finance/quote?region=US&lang=en&symbols=AAPL,TSLA,MARA,VYGR,PLTR", string.Empty)); //&symbols = AAPL,TSLA

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
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responsebody);

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

        public Task<StockData> GetStockData(string stockSymbol)
        {
            StockData stockData = new StockData();
            Uri uri = new Uri(string.Format(URL + $"/v6/finance/quote?region=US&lang=en&symbols={stockSymbol}", string.Empty));
            
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
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responsebody);

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
        public Task<List<StockData>> GetUserStockData(List<string> stockNames)
        {
            throw new NotImplementedException();
        }

        // TODO: Returns the value the stock sold for after calculating loss and gain
        public Task<decimal> SellStock(UserStockData userStock)
        {
            // Grab userStock object from the database
            //var userStockFromDB = GetUserStockData(userStock.StockDataID);

            // Need to get the current price of the stock
            //var currentStock = GetStockData(userStock.Symbol);

            // We need to assign the sell price to the ueerStock object and datatime of sale
            
            //decimal gainOrLoss = ((sellingPrice - buyingPrice) / buyingPrice) * 100;

            throw new NotImplementedException();
        }

    }
}
