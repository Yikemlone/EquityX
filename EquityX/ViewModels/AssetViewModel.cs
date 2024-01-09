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
        public ICommand SellStockCommand { get; set; }

        // Services
        private IStockService _stockService;

        public AssetViewModel(IStockService stockService)
        {
            _stockService = stockService;

            // Commands setup
            BuyStockCommand = new Command(() => BuyStock());
            SellStockCommand = new Command(() => SellStock());

            UserStocks = new ObservableCollection<UserStockData>();
            PercentageDifferences = new ObservableCollection<string>();
        }

        private async void BuyStock()
        {
            await Shell.Current.GoToAsync("BuyPage", new Dictionary<string, object>
            {
                ["StockData"] = StockData
            });
        }

        private async void SellStock()
        {
            await Shell.Current.GoToAsync("SellPage", new Dictionary<string, object>
            {
                ["StockData"] = StockData
            });
        }

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
                if (userStock.StockSymbol != StockData.Symbol)
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

        // TODO: The last things I need to implement
        // 1. The wacthlist
        // 2. The portfolio
        // 3. The search
        // 4. The sell 
        // 5. detailed asset evaluation



    }
}
