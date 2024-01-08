using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Pages;
using EquityX.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    /// <summary>
    /// Will be used to display stock information and be used to buy and sell stocks
    /// </summary>
    public partial class StockViewModel : ObservableObject
    {
        public ObservableCollection<StockData> StockData { get; set; }
         
        public ICommand BuyStockCommand { get; set; }
        public ICommand SellStockCommand { get; set; } 
        public Command<StockData> SelectionChangedCommand { get; set; }

        private IStockService _stockService; // Stock service to get stock data

        private IDispatcherTimer _timer; // Timer to update stock data

        public StockViewModel(IStockService stockService)
        {
            _stockService = stockService;
            StockData = new ObservableCollection<StockData>();

            // Commands setup
            SelectionChangedCommand = new Command<StockData>(SelectionChanged);

            // Get stock data
            UpdateStockData();
        }

        private async void SelectionChanged(StockData stockData)
        {
            await Shell.Current.GoToAsync($"{nameof(AssetPage)}");
        }

        // TODO: Add methods to buy and sell stocks
        private async void BuyStock()
        {
            throw new NotImplementedException();
        }

        private async void SellStock()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a timer to update the stock data on a set interval
        /// </summary>
        public void StockDataRefreshTimer()
        {
            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(10); // TODO: Update this later to 5 minutes/ maybe 30 seconds for demo
            _timer.Tick += (s, e) => UpdateStockData();
            _timer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void StopTimer() 
        {
            _timer?.Stop();
        }

        /// <summary>
        /// Get new stock data via the API call and updates the StockData
        /// </summary>
        private async void UpdateStockData()
        {
            var updatedStockData = await _stockService.GetStockData();
            StockData.Clear();

            foreach (var stock in updatedStockData)
            {
                StockData.Add(stock);
            }
        }
    }
}
