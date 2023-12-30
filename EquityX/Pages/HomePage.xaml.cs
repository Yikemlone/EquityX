using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		BindingContext = homeViewModel;
    }


	// TODO: Make a method that takes you to the StockPage

	// TODO: Make a method that takes you to the WatchlistPage

	// TODO: Make a method that takes you to the ProfilePage

	// TODO: Make a method that takes you to the AddFundsPage



}