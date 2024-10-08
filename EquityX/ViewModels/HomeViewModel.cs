﻿using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Context;
using EquityX.Models;
using EquityX.Pages;
using EquityX.Services;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.Collections.ObjectModel;
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

        public ObservableCollection<StockData> TopMoversData { get; set; }

        private IDispatcherTimer _timer;

        // Commands 
        public ICommand AddFundsCommand { get; private set; }
        public ICommand WithdrawCommand { get; private set; }
        public ICommand MoveToInvestPageCommand { get; private set; }
        public ICommand MoveToWatchlistPageCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; } 

        // Services
        private IFundsService _fundsService;
        private IStockService _stockService;
        private IAuthService _authService;
        private readonly EquityXDbContext _context;

        public HomeViewModel(
            IFundsService fundsService, 
            IStockService stockService,
            IAuthService authService,
            EquityXDbContext context)
        {
            TopMoversData = new ObservableCollection<StockData>();

            // Commands setup
            AddFundsCommand = new Command(() => AddFunds());
            WithdrawCommand = new Command(() => Withdraw());
            MoveToInvestPageCommand = new Command(() => GoToInvestingPage());
            MoveToWatchlistPageCommand = new Command(() => GoToWatchlistPage());
            LogoutCommand = new Command(() => Logout());

            // Services setup
            _fundsService = fundsService;
            _stockService = stockService;
            _context = context;
            _authService = authService;

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
                && await _fundsService.AddFunds(amount, Id))
            {
                AvailableFunds += amount;
                CalulatePortfolioValue();
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
                && await _fundsService.WithdrawFunds(amountToWithdraw, Id))
            {
                AvailableFunds -= amountToWithdraw;
                CalulatePortfolioValue();
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
                List<StockData> stockDatas = await _stockService.GetStockData();

                foreach (var stock in stockDatas)
                {
                    TopMoversData.Add(stock);
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"{e.Message}", "OK");
            }
        }

        /// <summary>
        /// Gets the user's data from the database
        /// </summary>
        public async void GetUserData()
        {
            int id = Preferences.Default.Get("USER_ID", 0);

            if (id == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Not logged in", "OK");
                return;
            }

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

        /// <summary>
        /// Moves the user to the SearchPage so they can invest in stocks
        /// </summary>
        private async void GoToInvestingPage()
        {
            if(DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                await Shell.Current.GoToAsync($"//{nameof(SearchPage)}");
            } else
            {
                await Shell.Current.GoToAsync($"//D{nameof(SearchPage)}");
            }
        }

        /// <summary>
        /// Moves the user to the WatchlistPage so they can view their watchlist
        /// </summary>
        private async void GoToWatchlistPage()
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                await Shell.Current.GoToAsync($"//{nameof(WatchlistPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//D{nameof(WatchlistPage)}");
            }   
        }

        /// <summary>
        /// Moves the user to the PortfolioPage so they can view their portfolio
        /// </summary>
        private async void Logout()
        {
            _authService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        /// <summary>
        /// Creates a timer to call the UpdateStockData method to update the stock data
        /// </summary>
        public void StockDataRefreshTimer()
        {
            _timer = Application.Current.Dispatcher.CreateTimer();
#if DEBUG
            _timer.Interval = TimeSpan.FromSeconds(1);

#else
            _timer.Interval = TimeSpan.FromSeconds(30); 
#endif

            _timer.Tick += (s, e) => UpdateStockData();
            _timer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void StopTimer()
        {
            _timer?.Stop();
        }

        /// <summary>
        /// Get new stock data via the API call and updates the TopMoversData
        /// </summary>
        private async void UpdateStockData()
        {
            var updatedStockData = await _stockService.GetStockData();
            TopMoversData.Clear();

            foreach (var stock in updatedStockData)
            {
                TopMoversData.Add(stock);
            }

            GetUserData();
            CalulatePortfolioValue();
        }

        /// <summary>
        /// Calaculates the user's portfolio value 
        /// </summary>
        private async void CalulatePortfolioValue()
        {
           PortfolioValue = await _stockService.CalulatePortfolioValue(Id);
        }
    }
}
