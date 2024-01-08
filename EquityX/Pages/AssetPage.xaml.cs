using EquityX.ViewModels;

namespace EquityX.Pages;

public partial class AssetPage : ContentPage
{
	public AssetPage(AssetViewModel assestViewModel)
	{
		InitializeComponent();
		BindingContext = assestViewModel;
		// May need to do the timer here 
	}
}