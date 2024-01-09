using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class SellPage : ContentPage
{
	private SellViewModel _sellViewModel;
	public SellPage(SellViewModel sellViewModel)
	{
		InitializeComponent();
		BindingContext = sellViewModel;
		_sellViewModel = sellViewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        _sellViewModel.GetCurrentStockData();
    }
}