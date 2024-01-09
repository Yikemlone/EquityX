using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Pages;
using EquityX.Services;
using System.Collections.ObjectModel;

namespace EquityX.ViewModels
{
    public partial class PortfolioViewModel : ObservableObject
    {
        // Properties
        public ObservableCollection<UserStockData> UserStockData { get; set; }

        // Commands
        public Command<UserStockData> SelectionChangedCommand { get; set; }

        // Services
        private IStockService _stockService;

        public PortfolioViewModel(IStockService stockService)
        {
            _stockService = stockService;
            UserStockData = new ObservableCollection<UserStockData>();

            // Commands setup   
            SelectionChangedCommand = new Command<UserStockData>(async (userStockData) => await SelectionChanged(userStockData));
        }

        async Task SelectionChanged(UserStockData userStockData) { 
            StockData stockData = await _stockService.GetStockDataBySymbol(userStockData.StockSymbol);

            await Shell.Current.GoToAsync($"{nameof(AssetPage)}", new Dictionary<string, object>
            {
                ["StockData"] = stockData
            });
        }

        public async void UpdateUserStockData()
        {
            int userID = Preferences.Default.Get("USER_ID", 0);

            if (userID == 0)
            {
                return;
            }

            List<UserStockData> userStockData = await _stockService.GetUserStockData(userID);
            UserStockData.Clear();

            foreach (UserStockData stockData in userStockData)
            {
                UserStockData.Add(stockData);
            }
        }
    }
}
