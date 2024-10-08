﻿using EquityX.Context;
using EquityX.DTO;
using EquityX.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

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
            user.PortfolioValue -= userStock.BuyInPrice;
            user.PortfolioValue += currecntStockData.SellPrice;

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
            string responsebody = await GetMockStockData();
            QuoteDTO myDeserializedClass = JsonConvert.DeserializeObject<QuoteDTO>(responsebody);
            Random random = new Random();


            foreach (var res in myDeserializedClass.QuoteResponse.Result)
            {
                int randomNumber = random.Next(-10, 11); // Simulate a random change in the stock price

                stockDataList.Add(new StockData()
                {
                    Name = res.longName,
                    BuyPrice = res.bid + randomNumber,
                    SellPrice = res.ask + randomNumber,
                    Currency = res.currency,
                    QuoteType = res.quoteType,
                    Symbol = res.symbol
                });
            }

            return stockDataList;
        }

        public async Task<List<StockData>> GetStockData(string symbols)
        {
            List<StockData> stockDataList = new List<StockData>();
            string responsebody = await GetMockStockData();
            QuoteDTO stockDataResponse = JsonConvert.DeserializeObject<QuoteDTO>(responsebody);
            Random random = new Random();

            List<string> symbolsList = symbols.Split(',').ToList();

            foreach (var symbol in symbolsList)
            {
                int randomInt = random.Next(-10, 11); // Simulate a random change in the stock price
                stockDataList.Add(new StockData()
                {
                    Name = stockDataResponse.QuoteResponse.Result.Where(s => s.symbol == symbol).Select(e => e.longName).FirstOrDefault(),
                    BuyPrice = stockDataResponse.QuoteResponse.Result.Where(s => s.symbol == symbol).Select(e => e.bid).FirstOrDefault() + randomInt,
                    SellPrice = stockDataResponse.QuoteResponse.Result.Where(s => s.symbol == symbol).Select(e => e.ask).FirstOrDefault() + (randomInt - 3),
                    Currency = stockDataResponse.QuoteResponse.Result.Where(s => s.symbol == symbol).Select(e => e.currency).FirstOrDefault(),
                    QuoteType = stockDataResponse.QuoteResponse.Result.Where(s => s.symbol == symbol).Select(e => e.quoteType).FirstOrDefault(),
                    Symbol = stockDataResponse.QuoteResponse.Result.Where(s => s.symbol == symbol).Select(e => e.symbol).FirstOrDefault()
                }); 
            }

            return stockDataList;
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
                .Where(u => u.UserID == userID && u.DateSold == null)
                .Select(e => e)
                .ToListAsync();

            if (userStockData.Count == 0)
            {
                return new List<UserStockData>();
            }
            
            return userStockData;
        }

        public async Task<List<UserWatchlist>> GetUserWatchlistData(int userID)
        {
            // Grab the user's stock data
            List<UserWatchlist> userWatchlist = await _context.UserWatchlist
                .Where(u => u.UserID == userID)
                .Select(e => e)
                .ToListAsync();

            if (userWatchlist.Count == 0)
            {
                return new List<UserWatchlist>();
            }

            return userWatchlist;
        }

        public async Task<string> GetTrendingStockData()
        {
            return await GetMockStockData();
        }

        public async Task<StockData> GetStockDataBySymbol(string symbol)
        {
            // This would be used in a search function, but since the data is static,
            // it will return the same stock data every time
            StockData stockData = new StockData();
            string responsebody = await GetMockStockData();

            QuoteDTO myDeserializedClass = JsonConvert.DeserializeObject<QuoteDTO>(responsebody);

            stockData.Name = myDeserializedClass.QuoteResponse.Result[0].longName;
            stockData.BuyPrice = myDeserializedClass.QuoteResponse.Result[0].bid;
            stockData.SellPrice = myDeserializedClass.QuoteResponse.Result[0].ask;
            stockData.Currency = myDeserializedClass.QuoteResponse.Result[0].currency;
            stockData.QuoteType = myDeserializedClass.QuoteResponse.Result[0].quoteType;
            stockData.Symbol = myDeserializedClass.QuoteResponse.Result[0].symbol;

            return stockData;
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

            if (rowsEffected == 0)
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

            if (rowsEffected == 0)
            {
                return false;
            }

            return true;
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

    }
}
