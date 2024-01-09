using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class WatchlistPage : ContentPage
{
	public WatchlistPage(WatchlistViewModel watchlistViewModel)
	{
		InitializeComponent();
		BindingContext = watchlistViewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        ((WatchlistViewModel)BindingContext).UpdateUserWatchlistData();
    }
}