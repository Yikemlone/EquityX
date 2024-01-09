using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Services;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    [QueryProperty(nameof(StockData), nameof(StockData))]
    public partial class BuyViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private StockData _stockData;

        [ObservableProperty]
        private int _quantity; // Leaveing this here for now, may not implement

        [ObservableProperty]
        private string _errorMessage;
        
        // Services
        private IStockService _stockService;

        // Commands
        public ICommand BuyCommand { get; set; }


        public BuyViewModel(IStockService stockService)
        {
            _stockService = stockService;

            // Commands setup
            BuyCommand = new Command(() => BuyStock());
        }

        private async void BuyStock()
        {
            int userID = Preferences.Default.Get("USER_ID", 0);

            if (userID == 0)
            {
                return;
            }

            bool transactionSucessful = await _stockService.BuyStock(StockData, userID);

            if (transactionSucessful)
            {
                await Shell.Current.GoToAsync("..");
            } 
            else
            {
                ErrorMessage = "Unable to make purchase, ensure you have enough available funds";
            }
        }
    }
}
