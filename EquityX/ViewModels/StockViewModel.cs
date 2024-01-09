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
        // Properties
        private IDispatcherTimer _timer; // Timer to update stock data

        [ObservableProperty]
        private string _searchText;

        public ObservableCollection<StockData> StockData { get; set; }

        // Commands
        public Command<StockData> SelectionChangedCommand { get; set; }

        // Services
        private IStockService _stockService; 

        public StockViewModel(IStockService stockService)
        {
            _stockService = stockService;
            StockData = new ObservableCollection<StockData>();

            // Commands setup
            SelectionChangedCommand = new Command<StockData>(async (stockData) => await SelectionChanged(stockData));

            UpdateStockData();
        }

        /// <summary>
        /// When a stock is selected, it will navigate to the AssetPage with the selected stock's data
        /// </summary>
        /// <param name="stockData"></param>
        Task SelectionChanged(StockData stockData) => 
            Shell.Current.GoToAsync($"{nameof(AssetPage)}", new Dictionary<string, object>
            {
                ["StockData"] = stockData
            });

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
