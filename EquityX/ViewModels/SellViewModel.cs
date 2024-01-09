using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Services;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    [QueryProperty(nameof(UserStockData), nameof(UserStockData))]
    public partial class SellViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private UserStockData _userStockData;

        [ObservableProperty]
        private StockData _currentData;

        [ObservableProperty]
        private decimal _profit;

        [ObservableProperty]
        private string _errorMessage;

        // Commands
        public ICommand SellCommand { get; set; }

        // Services
        private IStockService _stockService;

        public SellViewModel(IStockService stockService)
        {
            _stockService = stockService;
            CurrentData = new StockData();
            Profit = 0;

            // Commands setup
            SellCommand = new Command(() => SellStock());
        }

        private async void SellStock()
        {
            int userID = Preferences.Default.Get("USER_ID", 0);

            if (userID == 0)
            {
                return;
            }

            decimal transactionSucessful = await _stockService.SellStock(UserStockData, userID);

            if (transactionSucessful != 0)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ErrorMessage = "Unable to sell stock";
            }
        }

        public async void GetCurrentStockData()
        {
            CurrentData = await _stockService.GetStockDataBySymbol(UserStockData.StockSymbol);
            Profit = CurrentData.SellPrice - UserStockData.BuyInPrice;
        }
    }

}
