using EquityX.Services;

namespace EquityX.Pages;

public partial class LoadingPage : ContentPage
{
    IAuthService _authService;

	public LoadingPage(IAuthService authService)
	{
		InitializeComponent();
        _authService = authService;
	}

    // Doesn't work for mobile, but works for desktop
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (await _authService.IsAuthenticated())
        {
            await Shell.Current.GoToAsync($"//D{nameof(HomePage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//D{nameof(LoginPage)}");
        }
    }

    // Works for mobile
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (DeviceInfo.Idiom != DeviceIdiom.Phone) 
        {
            return;
        }

        if (await _authService.IsAuthenticated())
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }   
}