using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Context;
using EquityX.Models;
using EquityX.Pages;
using EquityX.Services;
using Microsoft.EntityFrameworkCore;
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
        private readonly EquityXDbContext _context;

        public HomeViewModel(IFundsService fundsService, 
            IStockService stockService,
            EquityXDbContext context)
        {
            // Commands setup
            AddFundsCommand = new Command(() => AddFunds());
            WithdrawCommand = new Command(() => Withdraw());
            InvestCommand = new Command(() => Invest());

            // Services setup
            _fundsService = fundsService;
            _stockService = stockService;
            _context = context;

            // Get the data from the API
            GetTopMoversData();
            GetUserData();
        }

        /// <summary>
        /// Adds funds to the user's account and updates the available funds
        /// </summary>
        private async void AddFunds()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Enter Number", "Please enter funds amount",
                placeholder:"$");

            if (Decimal.TryParse(result, out decimal amount) 
                && await _fundsService.ValidateFundsFromBank(amount))
            {
                AvailableFunds += amount;
            }
            else if(String.IsNullOrEmpty(result))
            {
                return;
            } 
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to add funds", "OK");
            }
        }

        /// <summary>
        /// Withdraws funds from the user's account and updates the available funds
        /// </summary>
        private async void Withdraw()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Enter Number", "Please enter amount to withdraw:",
                placeholder:"$");

            if (Decimal.TryParse(result, out decimal amountToWithdraw) 
                && await _fundsService.WithDrawFunds(amountToWithdraw, AvailableFunds))
            {
                AvailableFunds -= amountToWithdraw;
            }
            else if (String.IsNullOrEmpty(result))
            {
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to withdraw funds", "OK");
            }
        }

        /// <summary>
        /// Gets the top movers data from the API
        /// </summary>
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

        /// <summary>
        /// Moves the user to the SearchPage so they can invest
        /// </summary>
        private async void Invest()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SearchPage());
        }

        /// <summary>
        /// Gets the user's data from the database
        /// </summary>
        private async void GetUserData()
        {
            // TODO: Change this to get the user's ID from the login page
            // Probably need to pass it in as a parameter
            // Consider checking password here as well to ensure the user is valid
            int id = 1;

            User user = await _context.Users
                .Where(u => u.ID == id)
                .Select(e => e)
                .FirstOrDefaultAsync();

            Id = user.ID;
            Name = user.Name;
            PortfolioValue = user.PortfolioValue;
            AvailableFunds = user.AvailableFunds;
            UserStocks = user.UserStocks;
            UserWatchlist = user.UserWatchlist;
        }

        // TODO: Make a method that takes you to the WatchlistPage
        public async void GoToWatchlistPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new WatchlistPage());
        }

        // TODO: Make a method that takes you to the PortfolioPage
        public async void GoToPortfolioPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PortfolioPage());
        }   
    }
}
