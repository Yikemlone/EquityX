using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class SellPage : ContentPage
{
	public SellPage(SellViewModel sellViewModel)
	{
		InitializeComponent();
		BindingContext = sellViewModel;
	}
}