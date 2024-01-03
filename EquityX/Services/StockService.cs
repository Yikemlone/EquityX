using EquityX.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EquityX.Services
{
    public class StockService : IStockService
    {
        HttpClient _client;
        private string API_KEY = "ecEqpK0r3B2y6CWbr29Fq1DVnZz83IAq8LVzDUDa";
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
            

            throw new NotImplementedException();
        }

    }
}
