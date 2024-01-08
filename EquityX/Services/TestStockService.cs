﻿using EquityX.APIResponse.QuoteResponse;
using EquityX.Context;
using EquityX.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EquityX.Services
{
    /// <summary>
    /// This is a test stock service, only returning test values when in debug mode.
    /// </summary>
    public class TestStockService : IStockService
    {
        private readonly EquityXDbContext _context;

        public TestStockService(EquityXDbContext context)
        {
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

            if(rowsEffected == 0)
            {
                return false;
            }

            return true;
        }

        public Task<decimal> CalulatePortfolioValue(int userID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StockData>> GetStockData()
        {
            List<StockData> stockDataList = new List<StockData>();
            string responsebody = await GetMockStockData();

            QuoteRoot myDeserializedClass = JsonConvert.DeserializeObject<QuoteRoot>(responsebody);

            foreach (var res in myDeserializedClass.QuoteResponse.Result)
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

            return stockDataList;
        }

        public async Task<StockData> GetStockData(string symbol)
        {
            // This would be used in a search function, but since the data is static,
            // it will return the same stock data every time
            StockData stockData = new StockData();
            string responsebody = await GetMockStockData();

            QuoteRoot myDeserializedClass = JsonConvert.DeserializeObject<QuoteRoot>(responsebody);

            stockData.Name = myDeserializedClass.QuoteResponse.Result[0].longName;
            stockData.BuyPrice = myDeserializedClass.QuoteResponse.Result[0].bid;
            stockData.SellPrice = myDeserializedClass.QuoteResponse.Result[0].ask;
            stockData.Currency = myDeserializedClass.QuoteResponse.Result[0].currency;
            stockData.QuoteType = myDeserializedClass.QuoteResponse.Result[0].quoteType;
            stockData.Symbol = myDeserializedClass.QuoteResponse.Result[0].symbol;

            return stockData;
        }

        public Task<StockData> GetStockDataBySymbol(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTrendingStockData()
        {
            throw new NotImplementedException();
        }

        public Task<List<StockData>> GetUserStockData(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockData>> GetUserWatchlistData(int userID)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> SellStock(UserStockData userStock, int userID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the mock stock data from the Quote.txt file
        /// </summary>
        /// <returns>string</returns>
        private async Task<string> GetMockStockData()
        {
            // The Quote.txt file is a JSON formatted file that contains stock data, 
            // however it is EXTREMELY slow to be read in from the StreamReader,
            // so I am using it as a txt file for now 
            using var stream = await FileSystem.OpenAppPackageFileAsync("Quote.txt");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        Task<List<StockData>> IStockService.GetStockData(string symbols)
        {
            throw new NotImplementedException();
        }
    }
}
