using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Services;
using Microcharts;
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
        private ChartEntry[] _chartEntries;

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
            ChartEntry[] chartEntries = new ChartEntry[5];
        }
        
        private async void BuyStock()
        {
            await Shell.Current.GoToAsync("BuyPage", new Dictionary<string, object> 
            {
                ["StockData"] = StockData
            });

            int userID = Preferences.Default.Get("USER_ID", 0);

            if (userID == 0)
            {
                return;
            }

            bool buySucessful = await _stockService.BuyStock(StockData, userID);
        }

        private async void SellStock()
        {
            await Shell.Current.GoToAsync("SellPage", new Dictionary<string, object>
            {
                ["StockData"] = StockData
            });
        }


    }
}
