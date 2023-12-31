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


        // Commands 
        public ICommand AddFundsCommand { get; private set; }
        public ICommand InvestCommand { get; private set; }
        public ICommand WithdrawCommand { get; private set; }


        // Services
        IFundsService _fundsService;

        public HomeViewModel(IFundsService fundsService)
        {
            // TODO: Get this data from a JSON File or Database
            Id = 0;
            Name = "Mikey";
            PortfolioValue = 0;
            AvailableFunds = 0;
            UserStocks = new();
            UserWatchlist = new();

            // Commands setup
            //AddFundsCommand = new Command<decimal>((amount) => AddFunds(amount)); // Test method for code behind onlcick
            AddFundsCommand = new Command(async () => await AddFunds());
            WithdrawCommand = new Command(async () => await Withdraw());
            InvestCommand = new Command(async () => await Invest());

            // Services setup
            _fundsService = fundsService;
        }

        private async Task AddFunds()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Enter Number", "Please enter a number:");

            if (Decimal.TryParse(result, out decimal amount) && await _fundsService.ValidateFundsFromBank(amount))
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

            if (Decimal.TryParse(result, out decimal amountToWithdraw) && await _fundsService.WithDrawFunds(amountToWithdraw, AvailableFunds))
            {
                AvailableFunds -= amountToWithdraw;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to withdraw funds", "OK");
            }
        }

        private async Task Invest()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SearchPage());
        }


        // TODO: Make a method that takes you to the StockPage

        // TODO: Make a method that takes you to the WatchlistPage

        // TODO: Make a method that takes you to the ProfilePage

        // TODO: Make a method that takes you to the AddFundsPage


    }
}
