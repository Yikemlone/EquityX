using EquityX.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EquityX.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private decimal _portfolioValue;
        private decimal _availableFunds;
        private List<UserStockData> _userStocks;
        private List<UserWatchlist> _userWatchlist;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ID
        {
            get { return _id; }
            set 
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set 
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public decimal PortfolioValue
        {
            get { return _portfolioValue; }
            set 
            { 
                _portfolioValue = value; 
                NotifyPropertyChanged();
            }
        }

        public decimal AvailableFunds
        {
            get { return _availableFunds; }
            set 
            {
                _availableFunds = value;
                NotifyPropertyChanged();
            }
        }

        public List<UserStockData> UserStocks
        {
            get { return _userStocks; }
            set 
            {
                _userStocks = value; 
                NotifyPropertyChanged();
            }
        }

        public List<UserWatchlist> UserWatchlist
        {
            get { return _userWatchlist; }
            set 
            { 
                _userWatchlist = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand AddMoneyCommand { get; private set; }


        public HomeViewModel()
        {
            // TODO: Get this data from a JSON file
            ID = 0;
            Name = "Mikey";
            PortfolioValue = 0;
            AvailableFunds = 0;
            UserStocks = new();
            UserWatchlist = new();


            AddMoneyCommand = new Command<string>(AddMoney);
        }

        // TODO: Handle this via an Entry field instead of a button
        private void AddMoney(string amount)
        {
            AvailableFunds += Decimal.Parse(amount.ToString());
        }

        // TODO: Withdraw funds from the AvailableFunds property
        private void Withdraw()
        {
            throw new NotImplementedException();
        }

        // TODO: Bring the user to the Invest page
        private void Invest()
        {
            throw new NotImplementedException();
        }

        // Got this from https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-5.0
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
