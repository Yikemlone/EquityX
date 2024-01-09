using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class BuyPage : ContentPage
{
	public BuyPage(BuyViewModel buyViewModel)
	{
		InitializeComponent();
		BindingContext = buyViewModel;
	}
}