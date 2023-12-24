using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();

		var userDataViewModel = new HomeViewModel()
		{
			ID = 0,
			Name = "Mikey",
			PortfolioValue = 0,
			AvailableFunds = 0,
			UserStocks = new(),
			UserWatchlist = new()
        };

		BindingContext = userDataViewModel;
	}
}