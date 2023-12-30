using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using EquityX.Pages;
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


        public ICommand AddFundsCommand { get; private set; }
        public ICommand InvestCommand { get; private set; }
        public ICommand WithdrawCommand { get; private set; }


        public HomeViewModel()
        {
            // TODO: Get this data from a JSON file
            Id = 0;
            Name = "Mikey";
            PortfolioValue = 0;
            AvailableFunds = 0;
            UserStocks = new();
            UserWatchlist = new();

            // Commands setup
            AddFundsCommand = new Command(async () => await AddFunds());
            WithdrawCommand = new Command(async () => await Withdraw());
            InvestCommand = new Command(async () => await Invest());
        }

        private async Task Withdraw()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Enter Number", "Please enter a number:");
            if (decimal.TryParse(result, out decimal number))
            {
                AvailableFunds -= number;
            }
        }

        private async Task Invest()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SearchPage());
        }

        private async Task AddFunds()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Enter Number", "Please enter a number:");
            if (decimal.TryParse(result, out decimal number))
            {
                AvailableFunds += number;
            }
        }

    }
}
