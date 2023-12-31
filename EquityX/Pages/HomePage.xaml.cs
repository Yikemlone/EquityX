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

	// Leaving this here for now, but this is just a test method
	//public async void OnClicked_TestMethod(object sender, EventArgs e)
	//{
	//	if(decimal.TryParse(Money.Text, out decimal money)) 
	//	{
	//		_homeViewModel.AddFundsCommand.Execute(money);
	//	}
	//}

}