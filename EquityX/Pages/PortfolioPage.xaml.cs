using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class PortfolioPage : ContentPage
{
	public PortfolioPage(PortfolioViewModel portfolioViewModel)
	{
		InitializeComponent();
		BindingContext = portfolioViewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        ((PortfolioViewModel)BindingContext).UpdateUserStockData();
    }
}