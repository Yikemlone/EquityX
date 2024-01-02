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

        public StockService()
        {
            _client = new HttpClient();
        }

        public Task<bool> BuyStock(StockData stock, decimal availableFunds)
        {
            // Create the request with the API key header
            Uri uri = new Uri(string.Format(URL + $"/v6/finance/quote?region=US&lang=en&symbols={stock.Symbol}", string.Empty));
            StockData currentStockValue = null;

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

                currentStockValue = new StockData()
                {
                    Name = myDeserializedClass.quoteResponse.result[0].longName,
                    BuyPrice = myDeserializedClass.quoteResponse.result[0].bid,
                    SellPrice = myDeserializedClass.quoteResponse.result[0].ask,
                    Currency = myDeserializedClass.quoteResponse.result[0].currency,
                    QuoteType = myDeserializedClass.quoteResponse.result[0].quoteType,
                    Symbol = myDeserializedClass.quoteResponse.result[0].symbol
                };

                // Check if the user has enough funds to buy the stock
                if (currentStockValue == null || availableFunds < currentStockValue.BuyPrice)
                { 
                    Task.FromResult(false);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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

        public Task<StockData> GetStockData(string stockName)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockData>> GetUserStockData(List<string> stockNames)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> SellStock(int stockID)
        {
            throw new NotImplementedException();
        }

    }
}
