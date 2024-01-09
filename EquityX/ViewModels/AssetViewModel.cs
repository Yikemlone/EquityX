using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Services;
using Microcharts;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    [QueryProperty(nameof(StockData), nameof(StockData))]
    public partial class AssetViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private StockData _stockData;

        [ObservableProperty]
        private bool _hasStocks;

        public ObservableCollection<UserStockData> UserStocks { get; set; }
        public ObservableCollection<string> PercentageDifferences { get; set; }


        // Commands
        public ICommand BuyStockCommand { get; set; }
        public Command<UserStockData> SelectionChangedCommand { get; set; }

        // Services
        private IStockService _stockService;

        public AssetViewModel(IStockService stockService)
        {
            _stockService = stockService;

            // Commands setup
            BuyStockCommand = new Command(() => BuyStock());
            SelectionChangedCommand = new Command<UserStockData>(async (userStockData) => await SellStock(userStockData));

            UserStocks = new ObservableCollection<UserStockData>();
            PercentageDifferences = new ObservableCollection<string>();
        }

        /// <summary>
        /// Redirects the user to the buy page with the stock data
        /// </summary>
        Task BuyStock() => 
            Shell.Current.GoToAsync("BuyPage", new Dictionary<string, object>
            {
                ["StockData"] = StockData
            });

        /// <summary>
        /// Redirects the user to the sell page with the stock data
        /// </summary>
        Task SellStock(UserStockData userStockData) => 
            Shell.Current.GoToAsync("SellPage", new Dictionary<string, object>
            {
                ["UserStockData"] = userStockData
            });

        /// <summary>
        /// Gets the user's stock data for the current stock and calculates the percentage difference
        /// then, updates the UI if the user has stocks or not
        /// </summary>
        public async void GetUserStockData()
        {
            int userID = Preferences.Default.Get("USER_ID", 0);

            if (userID == 0)
            {
                return;
            }

            List<UserStockData> userStocks = await _stockService.GetUserStockData(userID);

            if (userStocks.Count < 0)
            {
                HasStocks = false;
                return;
            }

            UserStocks.Clear();

            foreach (UserStockData userStock in userStocks)
            {
                if (userStock.StockSymbol != StockData.Symbol || userStock.DateSold != null)
                {
                    continue;
                }

                UserStocks.Add(userStock);
            }

            PercentageDifferences.Clear();

            foreach (UserStockData userStock in UserStocks)
            {
                string percentageDifference = Math.Round((StockData.SellPrice - userStock.BuyInPrice) / userStock.BuyInPrice, 2).ToString();
                PercentageDifferences.Add(percentageDifference);
            }

            HasStocks = UserStocks.Count > 0;
        }
    }
}
