using EquityX.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EquityX.Services
{
    public class StockService : IStockService
    {
        HttpClient _client;
        private string API_KEY = "ecEqpK0r3B2y6CWbr29Fq1DVnZz83IAq8LVzDUDa";
        private string URL = "https://yfapi.net/v6/finance/quote?region=US&lang=en&symbols=AAPL,TSLA";

        public StockService()
        {
            _client = new HttpClient();
        }

        public Task<bool> BuyStock(StockData stock, decimal availableFunds)
        {
            //// Create the request with the API key header
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://yfapi.net/v6/finance/quote?region=US&lang=en&symbols=AAPL,TSLA");
            //request.Headers.Add("X-API-KEY", API_KEY);

            //// Send the request to the server
            //var task = _client.SendAsync(request);

            //// Check that the response is successful or throw an exception
            //var response = task.Result;

            //string responsebody = "";

            //if (response.EnsureSuccessStatusCode().StatusCode.Equals("200")) 
            //{
            //   responsebody = response.Content.ReadAsStringAsync().Result;
            //} 
            //else
            //{
            //    return Task.FromResult(false);
            //}

            //// Deserialize the JSON response
            //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(responsebody);

            //string assetSymbol = myDeserializedClass.quoteResponse.result[0].symbol;
            //decimal price = myDeserializedClass.quoteResponse.result[0].bid;

            throw new NotImplementedException();
            // return Task.FromResult(true);
        }

        public Task<List<StockData>> GetStockData()
        {
            List<StockData> stockDataList = new List<StockData>();
            Uri uri = new Uri(string.Format(URL + "&symbols = AAPL,TSLA", string.Empty)); //&symbols = AAPL,TSLA

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
