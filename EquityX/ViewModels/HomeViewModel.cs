using CommunityToolkit.Mvvm.ComponentModel;
using EquityX.Models;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    internal partial class HomeViewModel : ObservableObject
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


        public ICommand AddMoneyCommand { get; private set; }
        public ICommand AskForNumberCommand { get; private set; }


        public HomeViewModel()
        {
            // TODO: Get this data from a JSON file
            Id = 0;
            Name = "Mikey";
            PortfolioValue = 0;
            AvailableFunds = 0;
            UserStocks = new();
            UserWatchlist = new();

            AddMoneyCommand = new Command<string>(AddMoney);
            AskForNumberCommand = new Command(async () => await AskForNumber());
        }

        // TODO: Handle this via an Entry field instead of a button
        private void AddMoney(object amount)
        {
            if (amount is decimal decimalAmount)
            {
                AvailableFunds -= decimalAmount;
            }
        }

        // TODO: Withdraw funds from the AvailableFunds property
        private void Withdraw(object amount)
        {
            if (amount is decimal decimalAmount)
            {
                AvailableFunds -= decimalAmount;
            }
        }

        // TODO: Bring the user to the Invest page
        private void Invest()
        {
            throw new NotImplementedException();
        }

        private async Task AskForNumber()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Enter Number", "Please enter a number:");
            if (int.TryParse(result, out int number))
            {
                // Use the number here
            }
        }

    }
}
