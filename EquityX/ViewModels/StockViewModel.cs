using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
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

        private ICommand _buyStockCommand; // Command to buy a stock
        private ICommand _sellStockCommand; // Command to sell a stock

        private IStockService _stockService; // Stock service to get stock data

        private IDispatcherTimer _timer; // Timer to update stock data

        public StockViewModel(IStockService stockService)
        {
            _stockService = stockService;
            StockData = new ObservableCollection<StockData>();

            // Commands setup

            // Get stock data
            UpdateStockData();
        }

        // TODO: Add methods to buy and sell stocks

        public void StockDataRefreshTimer()
        {
            // This run no matter where you are in the app
            // this could be a problem for performance or too many API calls
            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(5); // Update this later to 5 minutes/ maybe 30 seconds for demo
            _timer.Tick += (s, e) => UpdateStockData();
            _timer.Start();
        }

        public void StopTimer() 
        {
            _timer?.Stop();
        }

        private async void UpdateStockData()
        {
            await Application.Current.MainPage.DisplayAlert("Timer", "Timer", "OK");

            //var updatedStockData = await _stockService.GetStockData();
            //StockData.Clear();

            //foreach (var stock in updatedStockData)
            //{
            //    StockData.Add(stock);      
            //}
        }
    }
}
