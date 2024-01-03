using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Services;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    /// <summary>
    /// Will be used to display stock information and be used to buy and sell stocks
    /// </summary>
    public partial class StockViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<StockData> _stockData; // List of stock data

        [ObservableProperty]
        private List<UserStockData> _userStockData; // List of user stock data

        private ICommand _buyStockCommand; // Command to buy a stock
        private ICommand _sellStockCommand; // Command to sell a stock

        private IStockService _stockService; // Stock service to get stock data

        StockViewModel(IStockService stockService)
        {
            _stockService = stockService;
        }

        // TODO: Add methods to buy and sell stocks

        // TODO: Add method to get stock data

    }
}
