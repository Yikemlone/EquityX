using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Services;
using Microcharts;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    [QueryProperty(nameof(StockData),nameof(StockData))]
    public partial class AssetViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private StockData _stockData;

        [ObservableProperty]
        private bool _hasStocks;

        public ObservableCollection<UserStockData> UserStocks { get; set; }

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

            // Get user stocks
            UserStocks = new ObservableCollection<UserStockData>();
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

            foreach (UserStockData userStock in userStocks)
            {
                if (userStock.StockSymbol != StockData.Symbol)
                {
                    continue;
                }

                UserStocks.Add(userStock);
            }

            HasStocks = UserStocks.Count > 0;
        }

    }
}
