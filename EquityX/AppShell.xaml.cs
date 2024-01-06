using EquityX.Pages;

namespace EquityX
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            //{
            //    Shell.Current.CurrentItem = PhoneTabs;
            //}
            
            // Routes for mobile
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(AssetPage), typeof(AssetPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(PortfolioPage), typeof(PortfolioPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(TransactionPage), typeof(TransactionPage));
            Routing.RegisterRoute(nameof(WatchlistPage), typeof(WatchlistPage));

            // Routes for desktop
            // D is for Desktop, if I used the word Desktop, it would try to open a file on your computer
            //Routing.RegisterRoute("D" + nameof(LoadingPage), typeof(LoadingPage));
            //Routing.RegisterRoute("D" + nameof(LoginPage), typeof(LoginPage));
            //Routing.RegisterRoute("D" + nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute("D" + nameof(AssetPage), typeof(AssetPage));
            Routing.RegisterRoute("D" + nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute("D" + nameof(PortfolioPage), typeof(PortfolioPage));
            Routing.RegisterRoute("D" + nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute("D" + nameof(TransactionPage), typeof(TransactionPage));
            Routing.RegisterRoute("D" + nameof(WatchlistPage), typeof(WatchlistPage));
        }
    }
}