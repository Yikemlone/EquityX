using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Pages;
using EquityX.Services;
using System.Collections.ObjectModel;

namespace EquityX.ViewModels
{
    public partial class WatchlistViewModel : ObservableObject
    {
        // Properties
        public ObservableCollection<UserWatchlist> UserStockData { get; set; }

        // Commands
        public Command<UserWatchlist> SelectionChangedCommand { get; set; }

        // Services
        private IStockService _stockService;

        public WatchlistViewModel(IStockService stockService)
        {
            _stockService = stockService;
            UserStockData = new ObservableCollection<UserWatchlist>();

            // Commands setup   
            SelectionChangedCommand = new Command<UserWatchlist>(async (userStockData) => await SelectionChanged(userStockData));
        }

        async Task SelectionChanged(UserWatchlist userStockData)
        {
            StockData stockData = await _stockService.GetStockDataBySymbol(userStockData.StockSymbol);

            await Shell.Current.GoToAsync($"{nameof(AssetPage)}", new Dictionary<string, object>
            {
                ["StockData"] = stockData
            });
        }

        public async void UpdateUserWatchlistData()
        {
            int userID = Preferences.Default.Get("USER_ID", 0);

            if (userID == 0)
            {
                return;
            }

            List<UserWatchlist> userStockData = await _stockService.GetUserWatchlistData(userID);
            UserStockData.Clear();

            foreach (UserWatchlist stockData in userStockData)
            {
                UserStockData.Add(stockData);
            }
        }
    }
}
