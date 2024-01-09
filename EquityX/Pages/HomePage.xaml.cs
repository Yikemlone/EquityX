using EquityX.ViewModels;
namespace EquityX.Pages;

public partial class HomePage : ContentPage
{
	private HomeViewModel _homeViewModel;

	public HomePage(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		BindingContext = homeViewModel;
		_homeViewModel = homeViewModel;
    }

    /// <summary>
    /// Starts the timer for refreshing the stock data when the page appears
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _homeViewModel.GetUserData();
        _homeViewModel.StockDataRefreshTimer();
    }

    /// <summary>
    /// Stops the timer for refreshing the stock data when the page disappears
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _homeViewModel.StopTimer();
    }

}