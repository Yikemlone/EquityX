using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class SearchPage : ContentPage
{
	private StockViewModel _stockViewModel;

	public SearchPage(StockViewModel stockViewModel)
	{
		InitializeComponent();
		BindingContext = stockViewModel;
        _stockViewModel = stockViewModel;
	}

    /// <summary>
    /// Starts the timer for refreshing the stock data when the page appears
    /// </summary>
	protected override void OnAppearing()
	{
        base.OnAppearing();
        _stockViewModel.StockDataRefreshTimer();
    }

    /// <summary>
    /// Stops the timer for refreshing the stock data when the page disappears
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _stockViewModel.StopTimer();
    }
}