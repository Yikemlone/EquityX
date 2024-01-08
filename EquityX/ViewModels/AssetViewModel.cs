using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Services;

namespace EquityX.ViewModels
{
    [QueryProperty(nameof(StockData),nameof(StockData))]
    public partial class AssetViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private StockData _stockData;

        // Services
        private IStockService _stockService;

        public AssetViewModel(IStockService stockService)
        {
            _stockService = stockService;
        }


    }
}
