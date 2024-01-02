using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Pages;
using EquityX.Services;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private decimal _portfolioValue;

        [ObservableProperty]
        private decimal _availableFunds;

        [ObservableProperty]
        private List<UserStockData> _userStocks;

        [ObservableProperty]
        private List<UserWatchlist> _userWatchlist;

        [ObservableProperty]
        private List<StockData> _topMoversData;


        // Commands 
        public ICommand AddFundsCommand { get; private set; }
        public ICommand InvestCommand { get; private set; }
        public ICommand WithdrawCommand { get; private set; }


        // Services
        private IFundsService _fundsService;
        private IStockService _stockService;

        public HomeViewModel(IFundsService fundsService, IStockService stockService)
        {
            // TODO: Get this data from a JSON File or Database
            Id = 0;
            Name = "Mikey";
            PortfolioValue = 0;
            AvailableFunds = 0;
            UserStocks = new();
            UserWatchlist = new();

            // Commands setup
            //AddFundsCommand = new Command<decimal>((amount) => AddFunds(amount)); // A test method for the code behind onlcick handler
            AddFundsCommand = new Command(async () => await AddFunds());
            WithdrawCommand = new Command(async () => await Withdraw());
            InvestCommand = new Command(async () => await Invest());

            // Services setup
            _fundsService = fundsService;
            _stockService = stockService;

            // Get the data from the API
            GetTopMoversData();

        }

        private async Task AddFunds()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Enter Number", "Please enter a number:");

            if (Decimal.TryParse(result, out decimal amount) 
                && await _fundsService.ValidateFundsFromBank(amount))
            {
                AvailableFunds += amount;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to add funds", "OK");
            }
        }

        private async Task Withdraw()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Enter Number", "Please enter a number:");

            if (Decimal.TryParse(result, out decimal amountToWithdraw) 
                && await _fundsService.WithDrawFunds(amountToWithdraw, AvailableFunds))
            {
                AvailableFunds -= amountToWithdraw;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to withdraw funds", "OK");
            }
        }

        private async void GetTopMoversData()
        {
            try
            {
                TopMoversData = _stockService.GetStockData().Result;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"{e.Message}", "OK");
            }
        }

        private async Task Invest()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SearchPage());
        }

        // TODO: Make a method that takes you to the WatchlistPage

        // TODO: Make a method that takes you to the PortfolioPage
    }
}
